using iParking.Domain.Entities.Auth;
using Microsoft.AspNetCore.Mvc;

namespace iParking.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [Route("api/v1/auth/login")]
        [HttpPost]
        public Task<IActionResult> Login([FromForm] LoginInput login)
        {

            if (login.KeySession == "123456789012345")// falta definir este login 
            {
                var response = new
                {
                    status = true,
                    data = new
                    {
                        keySession = "123456789012345",
                        rut = "11111111",
                        digVer = "k",
                        mail = "alvaro@ok.cl",
                        nombres = "ALVARO",
                        apellidos = "B.S",
                        telefono = "51 51213131"
                    }
                };

                return Task.FromResult<IActionResult>(Ok(response));
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
                return Task.FromResult<IActionResult>(BadRequest(response));
            }
        }

        [Route("api/v1/auth/logout")]
        [HttpPost]
        public IActionResult Logout([FromForm] LogoutInput logout)
        {
            if (logout.KeySession == "123456789012345")
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

