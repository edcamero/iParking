using iParking.DataAccess.Repositories;
using iParking.Domain.Entities.Auth;
using iParking.Domain.Shared;
using iParking.Infrastructure.Security;

namespace iParking.Application.Services.Auth
{
    /// <summary>
    /// Implementación del servicio de autenticación.
    /// Aplica principios SOLID y Clean Code:
    /// - Single Responsibility: Solo maneja lógica de autenticación
    /// - Dependency Inversion: Depende de abstracciones
    /// - Manejo adecuado de errores con Result Pattern
    /// </summary>
    public class AuthServices : IAuthServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityHash _securityHash;
        private readonly ITokenService _tokenService;

        public AuthServices(
            IUserRepository userRepository, 
            ISecurityHash securityHash,
            ITokenService tokenService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _securityHash = securityHash ?? throw new ArgumentNullException(nameof(securityHash));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<Result<bool>> LoginAsync(LoginInput login)
        {
            try
            {
                var hashedPassword = _securityHash.GenerateHash(login.ClaveAcceso);
                var isAuthenticated = await _userRepository.LoginAsync(login.Mail, hashedPassword);

                if (!isAuthenticated)
                {
                    return Result<bool>.Failure("Credenciales inválidas", 401);
                }

                return Result<bool>.Success(true, 200);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al intentar iniciar sesión: {ex.Message}", 500);
            }
        }
    }
}
