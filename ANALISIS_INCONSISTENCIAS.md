# Análisis de Inconsistencias del Dominio - iParking

## 1. Tabla de Equivalencias: Entidad C# <-> Tabla SQL <-> DTO

### 1.1 Usuarios

| Capa | Clase/Tabla | Campos | Problemas Detectados |
|------|-------------|--------|---------------------|
| **BD Legacy** | `TBL_USUARIOS` | `ID_USUARIO numeric(18,0)`, `RUT nvarchar(18)`, `DV nvarchar(1)`, `NOMBRES nvarchar(30)`, `APELLIDOS nvarchar(40)`, `TELEFONO nvarchar(40)`, `CLAVE_ACCESO nvarchar(150)`, `MAIL nvarchar(150)`, `ESTADO numeric(18,0)` | Tipos numéricos para IDs y ESTADO, nombres en UPPERCASE snake_case |
| **Domain Legacy** | `Usuario` (Entities/Usuario/) | `IdUsuario int`, `Rut string`, `Dv string`, `Nombres string`, `Apellidos string`, `Mail string`, `Telefono string`, `Estado int`, `ClaveAcceso string` | ✅ Mapeo correcto pero mezcla legacy con nuevo modelo |
| **Domain Nuevo** | `User` (Entities/MultiTenant/) | Hereda de `IdentityUser<int>`, agrega `Rut`, `Dv`, `Nombres`, `Apellidos`, `Telefono`, `CompanyId`, etc. | ✅ Modelo limpio para Identity |
| **DTO Entrada** | `UsuarioNuevo` | `Rut`, `DigVer`, `Mail`, `Nombres`, `Apellidos`, `Telefono`, `ClaveAcceso`, `Estado` | ⚠️ `Estado` no debería estar en DTO de creación |

### 1.2 Vehículos

| Capa | Clase/Tabla | Campos | Problemas Detectados |
|------|-------------|--------|---------------------|
| **BD Legacy** | `TBL_PLACA` | `ID_PLACA numeric(18,0)`, `PLACA nvarchar(6)`, `PLACA_DEFAULT numeric(18,0)`, `ID_USUARIO numeric(18,0)`, `FECHA_HORA_CREADO nvarchar(20)`, `ESTADO numeric(18,0)` | ⚠️ `FECHA_HORA_CREADO` como string, `PLACA_DEFAULT` es ID no placa |
| **BD Legacy** | `Vehicle` (EF Core) | `id int`, `plate varchar(255)`, `time bigint`, `parking varchar(255)`, `createdAt datetime`, `updatedAt datetime`, `state tinyint`, `parking_id int` | ⚠️ Modelo EF desactualizado vs TBL_PLACA |
| **Domain** | `EVehicle` | `IdPlaca decimal`, `Placa string`, `PlacaDefault decimal?`, `IdUsuario decimal?`, `FechaHoraCreado string`, `Estado decimal?` | ❌ TIPOS INCORRECTOS: decimal para IDs, string para fecha, decimal para estado |
| **DTO** | `VehicleUserInput` | `KeySession`, `ImeiPos`, `Placa`, `SerieCelular`, `VersionApp` | ✅ DTO de entrada OK |
| **DTO** | `VehicleUserInsert` | `UserId int`, `Placa string` | ✅ Simple OK |

### 1.3 Parqueaderos (Parking Lots)

| Capa | Clase/Tabla | Campos | Problemas Detectados |
|------|-------------|--------|---------------------|
| **BD Legacy** | `TBL_LUGARES` | `ID_LUGAR numeric`, `NOMBRE nvarchar(30)`, `LATITUD float`, `LONGITUD float`, `TARIFA nvarchar(50)`, `HORARIO nvarchar(50)`, `ESTADO bit` | ⚠️ LATITUD/LONGITUD como float (pérdida precisión), TARIFA/HORARIO como string |
| **BD Nuevo** | `Parking` (EF) | `id int`, `name varchar(255)`, `location varchar(255)`, `createdAt datetime`, `updatedAt datetime` | ⚠️ Modelo simple diferente a TBL_LUGARES |
| **Domain** | `ParkingLot` | `Id`, `CompanyId`, `Name`, `Address`, `City`, `Latitude decimal`, `Longitude decimal`, `OpeningTime TimeSpan`, `ClosingTime TimeSpan`, `Is24Hours bool`, `TotalCapacity int`, `IsActive bool` | ✅ Tipos correctos (decimal para coordenadas) |
| **DTO** | `ParkingInputDTO` | `Name`, `Location` | ⚠️ `Location` muy genérico, debería ser Address/City separado |

### 1.4 Tarjetas de Pago

| Capa | Clase/Tabla | Campos | Problemas Detectados |
|------|-------------|--------|---------------------|
| **BD Legacy** | `TBL_TARJETA` | `ID_TARJETA numeric`, `NUMERO_TARJETA numeric(18,0)`, `MES_VCTO numeric`, `ANO_VCTO numeric`, `CVV numeric`, `TIPO numeric`, `DEFAULT_TARJETA numeric`, `ID_USUARIO numeric`, `FECHA_HORA_CREADO nvarchar(20)`, `ESTADO numeric` | ❌ GRAVE: CVV almacenado (PCI-DSS violation), NUMERO_TARJETA como numeric |
| **Domain** | `ResponsePayDTO`, `ResponsePayKlap` | DTOs de respuesta de pago externo | ✅ Solo para comunicación externa |

### 1.5 Sesiones/Transacciones

| Capa | Clase/Tabla | Campos | Problemas Detectados |
|------|-------------|--------|---------------------|
| **BD Legacy** | `TBL_TRANSACCIONES` | `ID_TRANSACCION numeric`, `FECHANUM_ENTRADA numeric`, `FECHANUM_SALIDA numeric`, `HORA_ENTRADA nvarchar`, `HORA_SALIDA nvarchar`, `MINUTOS_ESTADIA numeric`, `MONTO_PAGADO numeric(18,0)`, `ID_TARJETA numeric`, `ID_PLACA numeric`, `ID_CELULAR numeric`, `FECHAHORA_TRANSACCION_CREADA nvarchar(20)` | ❌ Fechas como strings y números separados, MONTO_PAGADO sin decimales |
| **BD Legacy** | `TBL_SESIONES` | `ID_SESION numeric`, `KEY_SESSION numeric`, `ID_USUARIO numeric`, `FECHA_HORA_ULTIMO_ACCESO nvarchar(20)`, `FECHA_HORA_CREADO nvarchar(20)`, `ESTADO numeric`, `ID_CELULAR numeric` | ❌ Fechas como strings |
| **Domain** | `ParkingSession` | `Id`, `ParkingLotId`, `LicensePlate`, `VehicleType`, `EntryTime DateTime`, `ExitTime DateTime?`, `Status enum`, `TotalAmount decimal?`, `PaymentMethod enum?`, `IsPaid bool`, `TicketCode string` | ✅ Modelo correcto con tipos apropiados |

---

## 2. Problemas Críticos Detectados

### 2.1 Campos Duplicados con Distinto Nombre

| Entidad | Campo 1 | Campo 2 | capa | Corrección |
|---------|---------|---------|------|------------|
| Usuario | `ClaveAcceso` | - | Domain | ✅ Unificado |
| Usuario | `DigVer` (DTO) | `Dv` (Entity) | Inconsistencia naming | Renombrar a `Dv` en UsuarioNuevo |
| Vehicle | `Placa` | `Plate` (BD EF) | Legacy vs Nuevo | Mantener `Placa` en domain CL |
| Parking | `Location` (DTO) | `Address`+`City` (Entity) | DTO muy simple | Separar en DTO |

### 2.2 Tipos Primitivos Incorrectos

| Entidad | Campo | Tipo Actual | Tipo Correcto | Ubicación |
|---------|-------|-------------|---------------|-----------|
| EVehicle | `IdPlaca` | decimal | int | Domain |
| EVehicle | `FechaHoraCreado` | string | DateTime | Domain |
| EVehicle | `Estado` | decimal? | bool o enum | Domain |
| EVehicle | `IdUsuario` | decimal? | int? | Domain |
| EVehicle | `PlacaDefault` | decimal? | int? (es FK) | Domain |
| TBL_USUARIOS.ESTADO | `Estado` | numeric(18,0) | bit o tinyint | BD |
| TBL_TARJETA.NUMERO_TARJETA | Numero | numeric(18,0) | nvarchar(19) | BD (PCI) |
| TBL_TARJETA.CVV | CVV | numeric | NO ALMACENAR | BD (PCI) |
| TBL_TRANSACCIONES.MONTO_PAGADO | Monto | numeric(18,0) | decimal(18,2) | BD |
| TBL_LUGARES.LATITUD | Latitud | float | decimal(9,6) | BD |
| TBL_LUGARES.LONGITUD | Longitud | float | decimal(9,6) | BD |

### 2.3 Naming Inconsistente (Snake Case BD vs PascalCase C#)

| Tabla BD | Columna BD | Entity C# | Estado |
|----------|-----------|-----------|--------|
| TBL_USUARIOS | ID_USUARIO | IdUsuario | ⚠️ Debería ser Id |
| TBL_USUARIOS | RUT | Rut | ✅ |
| TBL_USUARIOS | DV | Dv | ✅ |
| TBL_USUARIOS | NOMBRES | Nombres | ✅ |
| TBL_USUARIOS | APELLIDOS | Apellidos | ✅ |
| TBL_USUARIOS | TELEFONO | Telefono | ✅ |
| TBL_USUARIOS | CLAVE_ACCESO | ClaveAcceso | ✅ |
| TBL_USUARIOS | MAIL | Mail | ✅ |
| TBL_USUARIOS | ESTADO | Estado | ✅ |
| TBL_PLACA | ID_PLACA | IdPlaca | ⚠️ Debería ser Id |
| TBL_PLACA | PLACA | Placa | ✅ |
| TBL_PLACA | FECHA_HORA_CREADO | FechaHoraCreado | ⚠️ Debería ser DateTime CreatedAt |

### 2.4 DTOs que Fugan Detalles de Persistencia

| DTO | Campo Problemático | Por qué es problema | Corrección |
|-----|-------------------|---------------------|------------|
| UsuarioNuevo | `Estado` | Campo interno de sistema, no lo define el usuario | Eliminar del DTO |
| UsuarioNuevo | `DigVer` | Debería llamarse `Dv` para consistencia | Renombrar a `Dv` |
| ParkingInputDTO | `Location` | Muy genérico, la entity tiene Address+City | Separar en Address, City |
| EVehicle | Todos los campos | No es DTO, es entidad con tipos incorrectos | Refactorizar o eliminar |

### 2.5 Enums Mapeados a INT sin Conversión Explícita

| Enum | Uso en BD | Conversión en Repository | Estado |
|------|-----------|-------------------------|--------|
| VehicleType | int | ❌ No hay conversión explícita en BaseRepository | Agregar helper |
| ParkingSessionStatus | int | ⚠️ Se usa directo en INSERT | Agregar conversión |
| PaymentMethod | int | ⚠️ Nullable sin validación | Agregar TryParse |
| ExcessType | int | - | Verificar mapeo |
| ExcessPaymentStatus | int | - | Verificar mapeo |
| UserRole | int | ✅ Identity lo maneja | OK |

---

## 3. Reglas de Negocio Violadas

1. **RUT/DV separados**: En BD están como columnas separadas (correcto), pero en EVehicle no existe este concepto.

2. **Estados como int**: TBL_USUARIOS.ESTADO es numeric cuando debería ser BIT (0/1).

3. **Fechas como string**: Multiple tablas legacy almacenan fechas como `nvarchar(20)` en formato "DD/MM/YYYY HH:MM:SS" aproximadamente.

4. **Monto sin decimales**: MONTO_PAGADO es numeric(18,0) - pierde centavos!

5. **PCI-DSS Violation**: TBL_TARJETA almacena CVV y número completo de tarjeta.

---

## 4. Plan de Corrección Prioritario

### PRIORIDAD 1: Eliminar EVehicle (tipos completamente wrong)
- Crear entidad `Vehicle` correcta en Domain/Entities/Vehicle/
- O mejor: usar directamente `ParkingSession` que ya tiene `LicensePlate` y `VehicleType`

### PRIORIDAD 2: Corregir UsuarioNuevo DTO
- Renombrar `DigVer` → `Dv`
- Eliminar `Estado` (no es input de usuario)
- Agregar validación de RUT chileno

### PRIORIDAD 3: Crear tipo compuesto `Rut`
```csharp
public record Rut(string Numero, string DigitoVerificador)
{
    public bool IsValid() => ValidarDV(Numero, DigitoVerificador);
    private static bool ValidarDV(string rut, string dv) { ... } // Algoritmo módulo 11
}
```

### PRIORIDAD 4: Agregar conversión de enums en BaseRepository
```csharp
protected TEnum MapEnum<TEnum>(object value) where TEnum : struct, Enum
{
    if (value == null || value == DBNull.Value) 
        return default;
    
    if (value is int intValue)
        return Enum.IsDefined(typeof(TEnum), intValue) 
            ? (TEnum)(object)intValue 
            : default;
    
    if (value is string stringValue)
        return Enum.TryParse<TEnum>(stringValue, true, out var result) 
            ? result 
            : default;
    
    return default;
}
```

### PRIORIDAD 5: Script SQL de migración para columnas críticas
- Cambiar MONTO_PAGADO a decimal(18,2)
- Cambiar LATITUD/LONGITUD a decimal(9,6)
- Eliminar CVV de TBL_TARJETA (si existe data real)
- Agregar columnas faltantes en tablas nuevas

