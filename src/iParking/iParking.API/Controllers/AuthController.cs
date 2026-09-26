using iParking.Application.Services.Auth;
using iParking.Application.Services.User;
using iParking.Domain.Entities.Auth;
using iParking.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace iParking.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthServices _authServices;
        private readonly IUserServices _userServices;
        private readonly ITokenService _tokenService;

        public AuthController(ILogger<AuthController> logger, IAuthServices authServices, IUserServices userServices, ITokenService tokenService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authServices = authServices ?? throw new ArgumentNullException(nameof(authServices));
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [Route("api/v1/auth/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginInput login)
        {
            var authResult = await _authServices.LoginAsync(login);

            if (authResult.IsSuccess)
            {
                var user = await _userServices.GetUser(login.Mail);

                if (user is null)
                {
                    return NotFound(ApiResponse<AuthResponse>.Failure("Usuario no encontrado"));
                }

                var response = new AuthResponse
                {
                    KeySession = _tokenService.GenerateToken(user.IdUsuario.ToString()),
                    FullName = $"{user.FirstName} {user.LastName}",
                    Mail = user.Email,
                };

                return Ok(ApiResponse<AuthResponse>.Success(response));
            }

            return BadRequest(ApiResponse<AuthResponse>.Failure(authResult.Error ?? "Usuario o clave Invalida!"));
        }

        [Route("api/v1/auth/logout")]
        [HttpPost]
        public IActionResult Logout([FromForm] LogoutInput logout)
        {
            var claimsPrincipal = _tokenService.ValidateToken(logout.KeySession);

            if (claimsPrincipal != null)
            {
                return Ok(ApiResponse<string>.Success("Sesión cerrada exitosamente"));
            }

            return BadRequest(ApiResponse<string>.Failure("Sesión inválida"));
        }
    }
}

