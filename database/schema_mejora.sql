-- Proposed schema based on user requirements
-- Tenants Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tenants]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tenants](
    [TenantId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [CreatedAt] [datetime] DEFAULT GETDATE(),
    [IsActive] [bit] DEFAULT 1,
    CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED ([TenantId] ASC)
)
END
GO

-- Roles Table (System Roles)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
    [RoleId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL, -- e.g., 'SuperAdmin', 'TenantAdmin', 'Operator'
    [Description] [nvarchar](255) NULL,
    [TenantId] [int] NULL, -- NULL for system-wide roles, set for custom tenant roles
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC),
    CONSTRAINT [FK_Roles_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([TenantId])
)
END
GO

-- SystemUsers Table (Management Users)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SystemUsers](
    [SystemUserId] [int] IDENTITY(1,1) NOT NULL,
    [TenantId] [int] NULL, -- NULL for SuperAdmins
    [RoleId] [int] NOT NULL,
    [Username] [nvarchar](50) NOT NULL,
    [PasswordHash] [nvarchar](255) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [IsActive] [bit] DEFAULT 1,
    CONSTRAINT [PK_SystemUsers] PRIMARY KEY CLUSTERED ([SystemUserId] ASC),
    CONSTRAINT [FK_SystemUsers_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([TenantId]),
    CONSTRAINT [FK_SystemUsers_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId])
)
END
GO

-- ParkingLots Table (Plazas/Sedes)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ParkingLots]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ParkingLots](
    [ParkingLotId] [int] IDENTITY(1,1) NOT NULL,
    [TenantId] [int] NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Latitude] [float] NOT NULL,
    [Longitude] [float] NOT NULL,
    [Address] [nvarchar](255) NULL,
    [OpeningHours] [nvarchar](100) NULL, -- e.g. "08:00 - 22:00"
    [IsActive] [bit] DEFAULT 1,
    CONSTRAINT [PK_ParkingLots] PRIMARY KEY CLUSTERED ([ParkingLotId] ASC),
    CONSTRAINT [FK_ParkingLots_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([TenantId])
)
END
GO

-- VehicleTypes Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VehicleTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[VehicleTypes](
    [VehicleTypeId] [int] IDENTITY(1,1) NOT NULL,
    [TenantId] [int] NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_VehicleTypes] PRIMARY KEY CLUSTERED ([VehicleTypeId] ASC),
    CONSTRAINT [FK_VehicleTypes_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([TenantId])
)
END
GO

-- Tariffs Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tariffs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tariffs](
    [TariffId] [int] IDENTITY(1,1) NOT NULL,
    [TenantId] [int] NOT NULL,
    [VehicleTypeId] [int] NOT NULL,
    [BillingType] [nvarchar](20) NOT NULL, -- Fraction, Hour, Day, Month
    [Price] [decimal](18, 2) NOT NULL,
    [ToleranceMinutes] [int] DEFAULT 0,
    CONSTRAINT [PK_Tariffs] PRIMARY KEY CLUSTERED ([TariffId] ASC),
    CONSTRAINT [FK_Tariffs_Tenants] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([TenantId]),
    CONSTRAINT [FK_Tariffs_VehicleTypes] FOREIGN KEY ([VehicleTypeId]) REFERENCES [dbo].[VehicleTypes] ([VehicleTypeId])
)
END
GO

-- VehicleOwners Table (Formerly TBL_USUARIOS) - For End Users
-- Assuming we modify the existing table or create a new one. 
-- Here we add TenantId to the existing structure concept.
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'TenantId' AND Object_ID = Object_ID(N'TBL_USUARIOS'))
BEGIN
    ALTER TABLE TBL_USUARIOS ADD TenantId INT NULL;
    ALTER TABLE TBL_USUARIOS ADD CONSTRAINT FK_TBL_USUARIOS_Tenants FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId);
END
GO

-- Add TenantId to Vehicle (Ingresos)
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'TenantId' AND Object_ID = Object_ID(N'Vehicle'))
BEGIN
    ALTER TABLE Vehicle ADD TenantId INT NULL;
    ALTER TABLE Vehicle ADD CONSTRAINT FK_Vehicle_Tenants FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId);
END
GO
