-- ============================================================================
-- SCRIPT DE MIGRACIÓN: Corrección de Inconsistencias del Dominio
-- iParking - Refactorización de modelos y tipos de datos
-- ============================================================================

BEGIN TRANSACTION;
GO

-- ----------------------------------------------------------------------------
-- 1. CORRECCIÓN: Tipos de columnas para precisión monetaria y geográfica
-- ----------------------------------------------------------------------------

-- 1.1 TBL_TRANSACCIONES.MONTO_PAGADO: numeric(18,0) → decimal(18,2)
-- Problema: Perdia centavos en los montos pagados
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TBL_TRANSACCIONES')
BEGIN
    ALTER TABLE [dbo].[TBL_TRANSACCIONES] 
    ALTER COLUMN [MONTO_PAGADO] DECIMAL(18, 2) NULL;
    
    PRINT '✓ TBL_TRANSACCIONES.MONTO_PAGADO convertido a DECIMAL(18,2)';
END
GO

-- 1.2 TBL_LUGARES.LATITUD/LONGITUD: float → decimal(9,6)
-- Problema: float pierde precisión para coordenadas geográficas
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TBL_LUGARES')
BEGIN
    -- Nota: Requiere convertir datos existentes
    ALTER TABLE [dbo].[TBL_LUGARES] 
    ALTER COLUMN [LATITUD] DECIMAL(9, 6) NULL;
    
    ALTER TABLE [dbo].[TBL_LUGARES] 
    ALTER COLUMN [LONGITUD] DECIMAL(9, 6) NULL;
    
    PRINT '✓ TBL_LUGARES.LATITUD y LONGITUD convertidos a DECIMAL(9,6)';
END
GO

-- 1.3 TBL_USUARIOS.ESTADO: numeric(18,0) → BIT
-- Problema: Estado es booleano (0/1), no necesita numeric grande
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TBL_USUARIOS')
BEGIN
    -- Primero convertir a tinyint intermedio (BIT no acepta NULL directamente en algunas versiones)
    ALTER TABLE [dbo].[TBL_USUARIOS] 
    ALTER COLUMN [ESTADO] TINYINT NULL DEFAULT 1;
    
    PRINT '✓ TBL_USUARIOS.ESTADO convertido a TINYINT';
END
GO

-- ----------------------------------------------------------------------------
-- 2. SEGURIDAD PCI-DSS: Eliminar almacenamiento de CVV
-- ----------------------------------------------------------------------------

-- 2.1 TBL_TARJETA.CVV: Eliminar columna (violación PCI-DSS)
-- ADVERTENCIA: Esto elimina datos. Asegurar backup previo.
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TBL_TARJETA')
AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TBL_TARJETA') AND name = 'CVV')
BEGIN
    ALTER TABLE [dbo].[TBL_TARJETA] DROP COLUMN [CVV];
    PRINT '✓ TBL_TARJETA.CVV eliminada (cumplimiento PCI-DSS)';
END
GO

-- 2.2 TBL_TARJETA.NUMERO_TARJETA: numeric → nvarchar(19)
-- Problema: Los números de tarjeta no son números matemáticos
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TBL_TARJETA')
BEGIN
    -- Convertir a string para preservar ceros iniciales y formato
    -- NOTA: En producción, considerar tokenización o eliminación completa
    ALTER TABLE [dbo].[TBL_TARJETA] 
    ALTER COLUMN [NUMERO_TARJETA] NVARCHAR(19) NULL;
    
    PRINT '✓ TBL_TARJETA.NUMERO_TARJETA convertido a NVARCHAR(19)';
END
GO

-- ----------------------------------------------------------------------------
-- 3. NUEVAS TABLAS: Modelo SaaS (si no existen)
-- ----------------------------------------------------------------------------

-- 3.1 Tabla Companies (Empresas clientes)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Companies')
BEGIN
    CREATE TABLE [dbo].[Companies] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [BusinessName] NVARCHAR(200) NOT NULL,
        [TaxId] NVARCHAR(20) NOT NULL,
        [LogoUrl] NVARCHAR(500) NULL,
        [ContactEmail] NVARCHAR(256) NULL,
        [ContactPhone] NVARCHAR(20) NULL,
        [Address] NVARCHAR(500) NULL,
        [City] NVARCHAR(100) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [SubscriptionStartDate] DATETIME NOT NULL,
        [SubscriptionEndDate] DATETIME NOT NULL,
        [MaxParkingLots] INT NOT NULL DEFAULT 5,
        [MaxUsers] INT NOT NULL DEFAULT 20,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NULL,
        [UpdatedBy] INT NULL
    );
    
    PRINT '✓ Tabla Companies creada';
END
GO

-- 3.2 Tabla ParkingLots (Parqueaderos por empresa)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ParkingLots')
BEGIN
    CREATE TABLE [dbo].[ParkingLots] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [CompanyId] INT NOT NULL FOREIGN KEY REFERENCES [Companies]([Id]) ON DELETE CASCADE,
        [Name] NVARCHAR(100) NOT NULL,
        [Address] NVARCHAR(500) NOT NULL,
        [City] NVARCHAR(100) NOT NULL,
        [Latitude] DECIMAL(9, 6) NOT NULL,
        [Longitude] DECIMAL(9, 6) NOT NULL,
        [OpeningTime] TIME NULL,
        [ClosingTime] TIME NULL,
        [Is24Hours] BIT NOT NULL DEFAULT 0,
        [TotalCapacity] INT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NULL
    );
    
    PRINT '✓ Tabla ParkingLots creada';
END
GO

-- 3.3 Tabla ParkingSpots (Puestos individuales)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ParkingSpots')
BEGIN
    CREATE TABLE [dbo].[ParkingSpots] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ParkingLotId] INT NOT NULL FOREIGN KEY REFERENCES [ParkingLots]([Id]) ON DELETE CASCADE,
        [Identifier] NVARCHAR(20) NOT NULL,  -- Ej: "A-12"
        [Sector] NVARCHAR(50) NOT NULL,      -- Ej: "Sector A"
        [VehicleType] TINYINT NOT NULL,      -- 1=Car, 2=Moto, 3=Truck, 4=Bicycle
        [Status] TINYINT NOT NULL DEFAULT 1, -- 1=Available, 2=Occupied, 3=Reserved, 4=Maintenance
        [IsDisabled] BIT NOT NULL DEFAULT 0, -- Para personas con discapacidad
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NULL
    );
    
    PRINT '✓ Tabla ParkingSpots creada';
END
GO

-- 3.4 Tabla ParkingRates (Tarifas configurables)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ParkingRates')
BEGIN
    CREATE TABLE [dbo].[ParkingRates] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ParkingLotId] INT NOT NULL FOREIGN KEY REFERENCES [ParkingLots]([Id]) ON DELETE CASCADE,
        [Name] NVARCHAR(100) NOT NULL,       -- Ej: "Tarifa Hora Auto"
        [RateType] TINYINT NOT NULL,         -- 1=Fractional, 2=Daily, 3=Overnight, 4=Subscription
        [VehicleType] TINYINT NOT NULL,      -- 1=Car, 2=Moto, etc.
        [Amount] DECIMAL(18, 2) NOT NULL,    -- Monto con decimales correctos
        [FreeMinutes] INT NOT NULL DEFAULT 0,
        [BillableMinutes] INT NULL,          -- NULL si es tarifa fija
        [IsActive] BIT NOT NULL DEFAULT 1,
        [StartDate] DATETIME NOT NULL,
        [EndDate] DATETIME NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NULL
    );
    
    PRINT '✓ Tabla ParkingRates creada';
END
GO

-- 3.5 Tabla ParkingSessions (Sesiones de estacionamiento)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ParkingSessions')
BEGIN
    CREATE TABLE [dbo].[ParkingSessions] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ParkingLotId] INT NOT NULL FOREIGN KEY REFERENCES [ParkingLots]([Id]),
        [LicensePlate] NVARCHAR(20) NOT NULL,
        [VehicleType] TINYINT NOT NULL,
        [EntryTime] DATETIME NOT NULL,
        [ExitTime] DATETIME NULL,
        [Status] TINYINT NOT NULL DEFAULT 1, -- 1=Active, 2=Completed, 3=Cancelled
        [ParkingSpotId] INT NULL FOREIGN KEY REFERENCES [ParkingSpots]([Id]) ON DELETE SET NULL,
        [OperatorUserId] INT NULL,           -- Usuario que operó la entrada/salida
        [RateId] INT NULL FOREIGN KEY REFERENCES [ParkingRates]([Id]) ON DELETE SET NULL,
        [TotalAmount] DECIMAL(18, 2) NULL,
        [PaymentMethod] TINYINT NULL,        -- 1=Cash, 2=DebitCard, 3=CreditCard, etc.
        [PaymentDate] DATETIME NULL,
        [IsPaid] BIT NOT NULL DEFAULT 0,
        [TicketCode] NVARCHAR(50) NOT NULL,  -- Código QR/Barras
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedAt] DATETIME NULL
    );
    
    PRINT '✓ Tabla ParkingSessions creada';
END
GO

-- ----------------------------------------------------------------------------
-- 4. ÍNDICES PARA MEJORAR RENDIMIENTO
-- ----------------------------------------------------------------------------

-- Índices en ParkingSessions para búsquedas comunes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ParkingSessions_LicensePlate')
BEGIN
    CREATE NONCLUSTERED INDEX IX_ParkingSessions_LicensePlate 
    ON [ParkingSessions]([LicensePlate]) 
    INCLUDE ([EntryTime], [Status]);
    PRINT '✓ Índice IX_ParkingSessions_LicensePlate creado';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ParkingSessions_Status')
BEGIN
    CREATE NONCLUSTERED INDEX IX_ParkingSessions_Status 
    ON [ParkingSessions]([Status]) 
    INCLUDE ([EntryTime], [LicensePlate]);
    PRINT '✓ Índice IX_ParkingSessions_Status creado';
END

-- ----------------------------------------------------------------------------
-- 5. VISTA DE COMPATIBILIDAD LEGACY (para migración gradual)
-- ----------------------------------------------------------------------------

-- Vista que unifica TBL_USUARIOS legacy con nuevo modelo
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_Usuarios_Completo')
BEGIN
    EXEC('CREATE VIEW [dbo].[vw_Usuarios_Completo] AS
    SELECT 
        u.ID_USUARIO,
        u.RUT,
        u.DV,
        u.NOMBRES,
        u.APELLIDOS,
        u.MAIL,
        u.TELEFONO,
        CASE WHEN u.ESTADO = 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS ESTADO_BIT,
        u.CLAVE_ACCESO,
        ''Legacy'' AS ORIGEN
    FROM [TBL_USUARIOS] u');
    
    PRINT '✓ Vista vw_Usuarios_Completo creada';
END
GO

COMMIT TRANSACTION;
PRINT '==============================================================================';
PRINT 'MIGRACIÓN COMPLETADA EXITOSAMENTE';
PRINT '==============================================================================';
GO
