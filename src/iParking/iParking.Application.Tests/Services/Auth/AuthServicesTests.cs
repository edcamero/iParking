using iParking.Application.Services.Auth;
using iParking.DataAccess.Repositories;
using iParking.Domain.Entities.Auth;
using iParking.Domain.Shared;
using iParking.Infrastructure.Security;
using Moq;
using Xunit;

namespace iParking.Application.Tests.Services.Auth
{
    /// <summary>
    /// Pruebas unitarias para el servicio de autenticación.
    /// Cubre los siguientes escenarios:
    /// - Login exitoso con credenciales válidas
    /// - Login fallido con credenciales inválidas
    /// - Manejo de excepciones durante el login
    /// </summary>
    public class AuthServicesTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISecurityHash> _securityHashMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthServices _authServices;

        public AuthServicesTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _securityHashMock = new Mock<ISecurityHash>();
            _tokenServiceMock = new Mock<ITokenService>();
            _authServices = new AuthServices(
                _userRepositoryMock.Object,
                _securityHashMock.Object,
                _tokenServiceMock.Object
            );
        }

        /// <summary>
        /// Prueba que el login retorna éxito cuando las credenciales son válidas.
        /// </summary>
        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var loginInput = new LoginInput
            {
                Mail = "test@example.com",
                ClaveAcceso = "password123"
            };
            var hashedPassword = "hashedPassword123";

            _securityHashMock
                .Setup(x => x.GenerateHash(loginInput.ClaveAcceso))
                .Returns(hashedPassword);

            _userRepositoryMock
                .Setup(x => x.LoginAsync(loginInput.Mail, hashedPassword))
                .ReturnsAsync(true);

            // Act
            var result = await _authServices.LoginAsync(loginInput);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
            Assert.Equal(200, result.Code);
            Assert.Empty(result.Error);
        }

        /// <summary>
        /// Prueba que el login retorna fallo cuando las credenciales son inválidas.
        /// </summary>
        [Fact]
        public async Task LoginAsync_WithInvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var loginInput = new LoginInput
            {
                Mail = "test@example.com",
                ClaveAcceso = "wrongPassword"
            };
            var hashedPassword = "hashedPassword123";

            _securityHashMock
                .Setup(x => x.GenerateHash(loginInput.ClaveAcceso))
                .Returns(hashedPassword);

            _userRepositoryMock
                .Setup(x => x.LoginAsync(loginInput.Mail, hashedPassword))
                .ReturnsAsync(false);

            // Act
            var result = await _authServices.LoginAsync(loginInput);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.IsFailure == false);
            Assert.Equal(401, result.Code);
            Assert.Equal("Credenciales inválidas", result.Error);
        }

        /// <summary>
        /// Prueba que el login maneja correctamente las excepciones.
        /// </summary>
        [Fact]
        public async Task LoginAsync_WithException_ReturnsFailure()
        {
            // Arrange
            var loginInput = new LoginInput
            {
                Mail = "test@example.com",
                ClaveAcceso = "password123"
            };
            var exceptionMessage = "Error de base de datos";

            _securityHashMock
                .Setup(x => x.GenerateHash(loginInput.ClaveAcceso))
                .Throws(new Exception(exceptionMessage));

            // Act
            var result = await _authServices.LoginAsync(loginInput);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(500, result.Code);
            Assert.Contains("Error al intentar iniciar sesión", result.Error);
            Assert.Contains(exceptionMessage, result.Error);
        }

        /// <summary>
        /// Prueba que el constructor lanza excepción cuando userRepository es null.
        /// </summary>
        [Fact]
        public void Constructor_WithNullUserRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AuthServices(
                null!,
                _securityHashMock.Object,
                _tokenServiceMock.Object
            ));
        }

        /// <summary>
        /// Prueba que el constructor lanza excepción cuando securityHash es null.
        /// </summary>
        [Fact]
        public void Constructor_WithNullSecurityHash_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AuthServices(
                _userRepositoryMock.Object,
                null!,
                _tokenServiceMock.Object
            ));
        }

        /// <summary>
        /// Prueba que el constructor lanza excepción cuando tokenService es null.
        /// </summary>
        [Fact]
        public void Constructor_WithNullTokenService_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AuthServices(
                _userRepositoryMock.Object,
                _securityHashMock.Object,
                null!
            ));
        }
    }
}
