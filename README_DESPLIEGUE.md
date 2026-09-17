# iParking - Guía de Uso y Despliegue Local

## Descripción del Proyecto

iParking es un sistema de gestión de estacionamiento desarrollado con arquitectura limpia en .NET 7/8, siguiendo principios SOLID y Clean Code. El proyecto está compuesto por:

- **Backend API**: .NET 7/8 con ASP.NET Core
- **Frontend**: React + TypeScript + Vite
- **Base de datos**: SQL Server

## Estructura del Proyecto

```
/workspace/src/iParking/
├── iParking.API/              # Capa de presentación (API REST)
├── iParking.Application/      # Lógica de negocio y servicios
├── iParking.DataAccess/       # Acceso a datos y repositorios
├── iParking.Domain/           # Entidades y modelos de dominio
├── iParking.Infrastructure/   # Implementaciones externas (JWT, hashing)
├── iParking.Web/              # Aplicación web Blazor
└── iParking.Application.Tests/# Pruebas unitarias
```

## Requisitos Previos

### Backend (.NET)

1. **.NET SDK 7.0 o superior**
   - Windows: [Descargar desde Microsoft](https://dotnet.microsoft.com/download)
   - Linux (Ubuntu/Debian):
     ```bash
     wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
     sudo dpkg -i packages-microsoft-prod.deb
     sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
     ```
   - macOS:
     ```bash
     brew install --cask dotnet-sdk
     ```

2. **SQL Server** (para desarrollo local)
   - SQL Server Express: [Descargar](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
   - O Docker:
     ```bash
     docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" \
       -p 1433:1433 --name sqlserver \
       -d mcr.microsoft.com/mssql/server:2022-latest
     ```

### Frontend (Node.js)

1. **Node.js 18+ y npm**
   - [Descargar desde Node.js](https://nodejs.org/)
   - Verificar instalación:
     ```bash
     node --version
     npm --version
     ```

## Configuración del Backend

### 1. Configurar la Cadena de Conexión

Edita el archivo `appsettings.Development.json` en `iParking.API/`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=iParking;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  },
  "JwtOptions": {
    "Key": "TuClaveSecretaMuyLargaYSegura123456!",
    "Issuer": "iParking.API",
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "ValidateLifetime": true,
    "ValidateIssuerSigningKey": true
  }
}
```

### 2. Crear la Base de Datos

```bash
cd /workspace/src/iParking/iParking.DataAccess

# Ejecutar migraciones para crear la base de datos
dotnet ef database update --startup-project ../iParking.API
```

Si no tienes EF Core tools instalados:
```bash
dotnet tool install --global dotnet-ef
```

### 3. Compilar el Proyecto

```bash
cd /workspace/src/iParking

# Restaurar paquetes NuGet
dotnet restore

# Compilar solución completa
dotnet build
```

## Ejecución en Local

### Backend API

```bash
cd /workspace/src/iParking/iParking.API

# Ejecutar en modo desarrollo
dotnet run

# O ejecutar en un puerto específico
dotnet run --urls="http://localhost:5000"
```

La API estará disponible en:
- Swagger UI: `http://localhost:5000/swagger`
- Endpoints: `http://localhost:5000/api/v1/*`

### Frontend

```bash
cd /workspace/frontend

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

El frontend estará disponible en: `http://localhost:5173`

## Ejecutar Pruebas Unitarias

El proyecto incluye pruebas unitarias para los servicios de aplicación.

### Ejecutar todas las pruebas

```bash
cd /workspace/src/iParking

# Ejecutar pruebas con xUnit
dotnet test iParking.Application.Tests/iParking.Application.Tests.csproj
```

### Ejecutar pruebas con detalles verbosos

```bash
dotnet test iParking.Application.Tests/iParking.Application.Tests.csproj --logger "console;verbosity=detailed"
```

### Ejecutar pruebas con cobertura de código

```bash
# Instalar coverlet si no está disponible
dotnet tool install --global coverlet.console

# Ejecutar con cobertura
dotnet test iParking.Application.Tests/iParking.Application.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

### Estructura de Pruebas

Las pruebas están organizadas por servicio:

```
iParking.Application.Tests/
├── Services/
│   ├── Auth/
│   │   └── AuthServicesTests.cs    # Pruebas de autenticación
│   ├── User/
│   │   └── UserServicesTests.cs    # (por implementar)
│   └── Parking/
│       └── ParkingServicesTests.cs # (por implementar)
```

### Ejemplo de Prueba Unitaria

```csharp
[Fact]
public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
{
    // Arrange
    var loginInput = new LoginInput
    {
        Mail = "test@example.com",
        ClaveAcceso = "password123"
    };
    
    _securityHashMock
        .Setup(x => x.GenerateHash(loginInput.ClaveAcceso))
        .Returns("hashedPassword123");
    
    _userRepositoryMock
        .Setup(x => x.LoginAsync(loginInput.Mail, "hashedPassword123"))
        .ReturnsAsync(true);

    // Act
    var result = await _authServices.LoginAsync(loginInput);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(200, result.Code);
}
```

## Endpoints de la API

### Autenticación

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/v1/auth/login` | Iniciar sesión |
| POST | `/api/v1/auth/logout` | Cerrar sesión |

### Ejemplo de Login con cURL

```bash
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "Mail=test@example.com&ClaveAcceso=password123"
```

## Solución de Problemas Comunes

### Error: "No se encuentra .NET SDK"

```bash
# Verificar instalación
dotnet --version

# Si no está instalado, seguir instrucciones de instalación arriba
```

### Error: "Cannot connect to database"

1. Verificar que SQL Server esté corriendo
2. Verificar cadena de conexión en `appsettings.Development.json`
3. Ejecutar migraciones:
   ```bash
   dotnet ef database update --startup-project ../iParking.API
   ```

### Error: "Port already in use"

Cambiar el puerto en `Properties/launchSettings.json`:
```json
"applicationUrl": "http://localhost:5001"
```

### Error: "npm ERR! code ENOENT"

```bash
# Limpiar caché de npm
npm cache clean --force

# Reinstalar dependencias
rm -rf node_modules package-lock.json
npm install
```

## Variables de Entorno (Opcional)

Para producción o configuración avanzada, usa variables de entorno:

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Server=localhost;Database=iParking;..."
export JwtOptions__Key="TuClaveSecreta"
```

## Docker (Opcional)

### Construir imagen del backend

```bash
cd /workspace/src/iParking/iParking.API
docker build -t iparking-api .
```

### Ejecutar con Docker Compose

```bash
cd /workspace/kuebernetes
# (Configurar archivos docker-compose.yml según sea necesario)
```

## Recursos Adicionales

- [Documentación de .NET](https://docs.microsoft.com/es-es/dotnet/)
- [Documentación de ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/)
- [Documentación de xUnit](https://xunit.net/docs)
- [Documentación de Moq](https://github.com/moq/moq4)

## Soporte

Para problemas o preguntas, revisar los archivos de documentación en la raíz del proyecto:
- `REFACTORING_SUMMARY.md`
- `PAYMENT_SECURITY_IMPROVEMENTS.md`
- `DATABASE_MIGRATION_SUMMARY.md`
