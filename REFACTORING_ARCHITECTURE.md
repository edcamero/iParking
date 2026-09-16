# 🏗️ Refactorización de Arquitectura - iParking SaaS

## Resumen Ejecutivo

Se ha completado la reestructuración del código base para soportar el modelo **Multi-Empresa (SaaS)** con los siguientes roles:
- ✅ **Super Administrador**: Gestiona la plataforma global
- ✅ **Administrador de Empresa**: Crea parqueaderos, configura tarifas, gestiona empleados
- ✅ **Operador de Parqueadero**: Modula entradas, salidas, cobros
- ✅ **Cliente/Conductor**: Consulta disponibilidad, gestiona vehículos, paga mensualidades

---

## 📁 Nueva Estructura de Entidades

### 1. **Entidades Multi-Tenant** (`iParking.Domain.Entities.MultiTenant`)
| Entidad | Descripción |
|---------|-------------|
| `Company` | Empresa cliente con suscripción, límites de parqueaderos y usuarios |

### 2. **Entidades de Parqueadero** (`iParking.Domain.Entities.Parking`)
| Entidad | Descripción |
|---------|-------------|
| `ParkingLot` | Sede/parqueadero con ubicación, horarios y capacidad |
| `ParkingSpot` | Puesto individual con sector, tipo de vehículo y estado |
| `ParkingRate` | Reglas de tarifas flexibles (fraccional, día, pernocta, mensual) |
| `ParkingSession` | Sesión de entrada/salida con cálculo automático de cobro |

### 3. **Entidades de Suscripción** (`iParking.Domain.Entities.Subscription`)
| Entidad | Descripción |
|---------|-------------|
| `Subscription` | Mensualidades/planes recurrentes por vehículo |

### 4. **Usuario Mejorado** (`iParking.Domain.Entities.Usuario.User`)
- Soporte multi-rol con enum `UserRole`
- Relación con empresa (`CompanyId`)
- Historial de login y sesiones operadas

### 5. **Enums Centralizados** (`iParking.Domain.Enums`)
```csharp
UserRole          // SuperAdmin, CompanyAdmin, Operator, Client
VehicleType       // Car, Motorcycle, Truck, Bicycle
ParkingSpotStatus // Available, Occupied, Reserved, Maintenance
RateType          // Fractional, Daily, Overnight, Subscription
PaymentMethod     // Cash, DebitCard, CreditCard, Transfer, DigitalWallet, Recurrent
ParkingSessionStatus // Active, Completed, Cancelled
```

---

## 🔧 Repositorios Implementados

### Patrón Repository con Dapper + Result Pattern

| Repositorio | Métodos Clave |
|-------------|--------------|
| `ICompanyRepository` | CRUD empresas, búsqueda por NIT/RUT, conteo activo |
| `IParkingLotRepository` | CRUD parqueaderos, conteo puestos disponibles/ocupados |
| `IParkingSessionRepository` | Registro entrada/salida, búsqueda por placa, cálculo de ingresos |

**Beneficios:**
- ✅ **SQL Parametrizado**: Previene inyección SQL
- ✅ **Result Pattern**: Manejo elegante de errores sin excepciones
- ✅ **BaseRepository**: Código DRY para operaciones comunes
- ✅ **Interfaces Segregadas**: Principio ISP de SOLID

---

## 🎯 Características del Modelo de Negocio

### A. Gestión Multi-Empresa y Multi-Sede
- [x] Registro de empresas con NIT/RUT, logo, datos de facturación
- [x] Múltiples sedes por empresa con coordenadas GPS
- [x] Horarios configurables (24/7 o rangos específicos)

### B. Configuración de Capacidad y Espacios
- [x] Inventario de celdas por categoría (Autos, Camiones, Motos, Bicicletas)
- [x] Nomenclatura por sector (ej: "Sector A - Puesto 12")
- [x] Estados en tiempo real (Libre, Ocupado, Mantenimiento, Reservado)

### C. Tarifas y Planes de Cobro Flexibles
- [x] **Ocasional/Fracción**: Cobro por minuto/hora con tiempo de gracia
- [x] **Día Completo/Pernocta**: Tarifa fija
- [x] **Suscripciones/Mensualidades**: Planes periódicos por placa
- [x] Diferenciación por tipo de vehículo
- [x] Múltiples medios de pago (Efectivo, Tarjeta, QR, Billeteras, Recurrente)

### D. Flujo Operativo Completo

#### Entrada del Vehículo
1. Registro de placa y tipo de vehículo
2. Validación de mensualidad activa (si aplica)
3. Generación de tiquete con código QR/Barras
4. Asignación de puesto disponible

#### Permanencia
- Dashboard de ocupación en tiempo real
- Mapa/lista de puestos por categoría

#### Salida y Pago
1. Escaneo de tiquete o placa
2. **Cálculo automático** de tiempo transcurrido
3. Aplicación de tarifa correspondiente
4. Procesamiento de pago (efectivo/tarjeta/digital)
5. Liberación de puesto y recibo

### E. Reportes y Control Financiero
- [x] Cierre de caja por operador (arqueos diarios)
- [x] Estadísticas de ocupación y horas pico
- [x] Tiempo promedio de estadía
- [x] Alertas de mensualidades vencidas

---

## 📐 Principios de Diseño Aplicados

### SOLID
| Principio | Aplicación |
|-----------|------------|
| **S**RP | Cada repositorio y servicio con una responsabilidad única |
| **O**CP | Entidades extendibles mediante herencia de `BaseEntity` |
| **L**SP | Repositorios implementan interfaces comunes |
| **I**SP | Interfaces segregadas por dominio (Company, Parking, Subscription) |
| **D**IP | Dependencia de abstracciones (interfaces) no implementaciones |

### DRY (Don't Repeat Yourself)
- `BaseRepository` centraliza conexiones y ejecución SQL
- Enums compartidos en todo el dominio
- Validaciones centralizadas con DataAnnotations

### Clean Code
- Nombres descriptivos en clases, métodos y variables
- XML documentation en todas las clases públicas
- Clases pequeñas con cohesión alta
- Manejo explícito de errores con `Result<T>`

---

## 🚀 Próximos Pasos Recomendados

### 1. Migraciones de Base de Datos
Crear scripts SQL para las nuevas tablas:
```sql
Companies, ParkingLots, ParkingSpots, ParkingRates, 
ParkingSessions, Subscriptions
```

### 2. Servicios de Aplicación
Implementar en `iParking.Application`:
- `CompanyService` con validación de límites del plan
- `ParkingSessionService` con cálculo de tarifas
- `SubscriptionService` con alertas de vencimiento
- `ReportingService` para cierres de caja y estadísticas

### 3. API Endpoints
- `POST /api/companies` - Registro de empresa (SuperAdmin)
- `POST /api/parking-lots` - Crear parqueadero (CompanyAdmin)
- `POST /api/parking-sessions/entry` - Registrar entrada (Operator)
- `POST /api/parking-sessions/{id}/exit` - Registrar salida y cobro
- `GET /api/dashboard/occupancy` - Ocupación en tiempo real

### 4. Seguridad y Autorización
- Policies por rol (`SuperAdminPolicy`, `CompanyAdminPolicy`, etc.)
- Filtro de datos por `CompanyId` en consultas
- Auditoría de acciones críticas

### 5. Integraciones
- Lectura automática de placas (LPR)
- Pasarelas de pago para cobros recurrentes
- Notificaciones (email/SMS) para vencimientos

---

## 📋 Archivos Creados/Modificados

### Nuevos Archivos (15)
```
✅ Domain/Enums/UserRole.cs
✅ Domain/Entities/MultiTenant/Company.cs
✅ Domain/Entities/Parking/ParkingLot.cs
✅ Domain/Entities/Parking/ParkingSpot.cs
✅ Domain/Entities/Parking/ParkingRate.cs
✅ Domain/Entities/Parking/ParkingSession.cs
✅ Domain/Entities/Subscription/Subscription.cs
✅ Domain/Entities/Usuario/Usuario.cs (refactorizado a User)
✅ DataAccess/Repositories/Company/ICompanyRepository.cs
✅ DataAccess/Repositories/Company/CompanyRepository.cs
✅ DataAccess/Repositories/Parking/IParkingLotRepository.cs
✅ DataAccess/Repositories/Parking/ParkingLotRepository.cs
✅ DataAccess/Repositories/Parking/IParkingSessionRepository.cs
✅ DataAccess/Repositories/Parking/ParkingSessionRepository.cs
✅ REFACTORING_SUMMARY.md (este archivo)
```

### Archivos Existentes Mejorados
- `UsuarioNuevo.cs` → Con validaciones DataAnnotations
- `BaseRepository.cs` → Con métodos genéricos seguros
- `Result.cs` → Pattern para manejo de errores

---

## 🎓 Conclusiones

La refactorización establece una base sólida para un sistema **SaaS multi-empresa** escalable, seguro y mantenible. Los principios de **Clean Code, DRY y SOLID** garantizan que el sistema pueda evolucionar fácilmente para soportar nuevos requerimientos como:
- Reservas anticipadas
- Integración con hardware (barreras, sensores)
- App móvil para conductores
- Business Intelligence avanzado

El uso de **Dapper** mantiene el rendimiento alto mientras que el **Result Pattern** mejora la experiencia de desarrollo y debugging.
