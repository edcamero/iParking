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
            var isAuth = await _authServices.Login(login);

            if (isAuth)
            {
                var user = await _userServices.GetUser(login.Mail);

                if (user is null) {
                    throw new Exception("no existe");
                }

                var response = new
                {
                    status = true,
                    data = new
                    {
                        keySession = _tokenService.GenerateToken(user.IdUsuario.ToString()),
                        rut = user.Rut,
                        digVer = user.Dv,
                        mail = user.Mail,
                        nombres = user.Nombres,
                        apellidos = user.Apellidos,
                        telefono = user.Telefono,
                    }
                };

                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        errorMessage = "Usuario o clave Invalida!"
                    }
                };
                return BadRequest(response);
            }
        }

        [Route("api/v1/auth/logout")]
        [HttpPost]
        public IActionResult Logout([FromForm] LogoutInput logout)
        {
            var claimsPrincipal = _tokenService.ValidateToken(logout.KeySession);

            if (claimsPrincipal != null)
            {
                return Ok(new { estatus = true });
            }
            else
            {
                return BadRequest(new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        errorMessage = "Usuario o clave Invalida!"
                    }
                });
            }
        }
    }
}

