# Migración de Base de Datos - iParking SaaS

## Resumen Ejecutivo

Se ha completado la implementación del esquema completo de base de datos para el sistema iParking, migrando desde una estructura básica hacia un modelo SaaS multi-empresa con ASP.NET Core Identity.

## Cambios Principales

### 1. **ASP.NET Core Identity Integrado**

El sistema ahora utiliza `IdentityDbContext<User, IdentityRole<int>, int>` proporcionando:
- ✅ Hash automático de contraseñas (PBKDF2)
- ✅ Protección contra ataques de fuerza bruta (Lockout)
- ✅ Sistema de roles y claims integrado
- ✅ Tokens de seguridad para recuperación de cuenta
- ✅ Confirmación de email y 2FA opcional

**Tabla Users incluye:**
- Campos Identity estándar: `UserName`, `Email`, `PasswordHash`, `SecurityStamp`, etc.
- Campos custom del negocio: `Rut`, `Dv`, `Nombres`, `Apellidos`, `CompanyId`, `Role`, `ImeiCelular`, etc.

### 2. **Modelo Multi-Empresa (SaaS)**

**Tabla Companies:**
```sql
- Id (PK)
- BusinessName, TaxId (NIT/RUT)
- LogoUrl, ContactEmail, ContactPhone
- Address, City
- IsActive, SubscriptionStartDate, SubscriptionEndDate
- MaxParkingLots, MaxUsers (límites por plan)
```

### 3. **Gestión de Parqueaderos Multi-Sede**

**Tabla ParkingLots:**
```sql
- Id (PK), CompanyId (FK)
- Name, Address, City
- Latitude, Longitude (coordenadas GPS)
- OpeningTime, ClosingTime, Is24Hours
- TotalCapacity, IsActive
```

### 4. **Configuración de Capacidad y Espacios**

**Tabla ParkingSpots:**
```sql
- Id (PK), ParkingLotId (FK)
- Identifier (ej: "A-12"), Sector (ej: "Sector A")
- VehicleType (Car, Motorcycle, Truck, Bicycle)
- Status (Available, Occupied, Reserved, Maintenance)
- IsDisabled (para personas con discapacidad)
```

### 5. **Tarifas Flexibles**

**Tabla ParkingRates:**
```sql
- Id (PK), ParkingLotId (FK)
- Name, RateType (Fractional, Daily, Overnight, Subscription)
- VehicleType, Amount
- FreeMinutes, BillableMinutes
- IsActive, StartDate, EndDate
```

**Tipos de tarifa soportados:**
1. **Fraccional**: Cobro por minuto/hora con tiempo de gracia
2. **Día Completo**: Tarifa fija por día
3. **Pernocta**: Tarifa especial nocturna
4. **Suscripción**: Planes mensuales/quincenales

### 6. **Flujo Operativo (Entrada/Salida)**

**Tabla ParkingSessions:**
```sql
- Id (PK), ParkingLotId (FK)
- LicensePlate, VehicleType
- EntryTime, ExitTime, Status
- ParkingSpotId (FK nullable), OperatorUserId (FK nullable)
- RateId (FK nullable), TotalAmount, PaymentMethod
- PaymentDate, IsPaid, TicketCode (QR/Barras)
```

**Estados de sesión:**
- `Active`: Vehículo estacionado
- `Completed`: Salida registrada y pagada
- `Cancelled`: Sesión cancelada

### 7. **Suscripciones y Mensualidades**

**Tabla Subscriptions:**
```sql
- Id (PK), CompanyId (FK), ParkingLotId (FK nullable)
- LicensePlate, VehicleType, UserId (FK)
- PlanType, Amount, StartDate, EndDate
- IsActive, AutoRenew
- LastPaymentDate, NextPaymentDate, GracePeriodDays
```

### 8. **Medios de Pago (Sin Almacenar Tarjetas)**

**Importante**: El sistema NO almacena datos de tarjetas de crédito/débito.

**PaymentMethod enum:**
1. `Cash` - Efectivo
2. `DebitCard` - Débito (procesado externamente via Stripe/PayPal/ePayco)
3. `CreditCard` - Crédito (procesado externamente)
4. `Transfer` - Transferencia/QR
5. `DigitalWallet` - Billeteras digitales
6. `Recurrent` - Cobro recurrente con token de pasarela

**Cumplimiento PCI-DSS**: ✅ Los datos sensibles son manejados exclusivamente por las pasarelas de pago externas.

## Roles de Usuario

| Role ID | Nombre | Descripción |
|---------|--------|-------------|
| 1 | SuperAdmin | Gestiona la plataforma SaaS global |
| 2 | CompanyAdmin | Admin de empresa (crea parqueaderos, ve reportes) |
| 3 | Operator | Operador de sitio (entradas/salidas/cobros) |
| 4 | Client | Conductor (consulta, paga mensualidades) |

## Tipos de Vehículo

| Type ID | Nombre |
|---------|--------|
| 1 | Car |
| 2 | Motorcycle |
| 3 | Truck |
| 4 | Bicycle |

## Estructura de Índices

La migración crea índices optimizados para:
- Búsquedas por empresa (`IX_Users_CompanyId`, `IX_ParkingLots_CompanyId`)
- Consultas de sesiones activas (`IX_ParkingSessions_ParkingLotId`, `IX_ParkingSessions_Status`)
- Búsquedas de usuarios (`EmailIndex`, `UserNameIndex`)
- Integridad referencial (`IX_UserRoles_RoleId`, `IX_Subscriptions_UserId`)

## Instrucciones de Uso

### Ejecutar la Migración

```bash
cd /workspace/src/iParking/iParking.API

# Si tienes dotnet CLI instalado:
dotnet ef database update

# O mediante Package Manager Console en Visual Studio:
Update-Database
```

### Seed Inicial de Datos (Recomendado)

Crear un script para:
1. Crear rol SuperAdmin
2. Crear usuario administrador inicial
3. Configurar empresas demo

## Diagrama Entidad-Relación

```
Companies (1) ──────< Users (>1)
    │                     │
    │                     └───> ParkingSessions (OperatedSessions)
    │
    └───< ParkingLots (1)
              │
              ├───< ParkingSpots (>1)
              │
              ├───< ParkingRates (>1)
              │
              ├───< ParkingSessions (>1)
              │         │
              │         ├───> ParkingSpot (nullable)
              │         ├───> User (OperatorUser, nullable)
              │         └───> ParkingRate (nullable)
              │
              └───< Subscriptions (>1)
                        │
                        └───> User (ClientId)
```

## Próximos Pasos Recomendados

1. **Configurar conexión a SQL Server** en `appsettings.json`
2. **Ejecutar migración** para crear esquema
3. **Implementar seed data** para roles y usuario admin
4. **Integrar pasarela de pagos** (Stripe, PayPal, ePayco, MercadoPago)
5. **Configurar autenticación JWT** en la API
6. **Implementar policies de autorización** basadas en roles

## Notas de Seguridad

- ✅ Contraseñas hasheadas con PBKDF2 (Identity)
- ✅ Sin almacenamiento de datos de tarjetas (PCI-DSS compliant)
- ✅ Foreign keys con reglas de cascada apropiadas
- ✅ Índices únicos para emails y usernames
- ✅ Campos sensibles con longitud máxima adecuada

## Archivos Modificados/Creados

| Archivo | Tipo | Descripción |
|---------|------|-------------|
| `ParkingContext.cs` | Modificado | Ahora hereda de `IdentityDbContext<User, IdentityRole<int>, int>` |
| `Usuario.cs` | Modificado | Ahora hereda de `IdentityUser<int>` |
| `iParking.DataAccess.csproj` | Modificado | Agregado package `Microsoft.AspNetCore.Identity.EntityFrameworkCore` |
| `20250116000000_InitialCreate.cs` | Creado | Migración inicial completa |

---

**Fecha de creación**: Enero 2025  
**Versión**: 1.0.0  
**Framework**: .NET 7.0  
**Provider**: Entity Framework Core 7.0.5 + SQL Server
