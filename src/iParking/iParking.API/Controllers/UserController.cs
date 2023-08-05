using iParking.Application.Services.User;
using iParking.Domain.Entities.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace iParking.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _userServices;

        public UserController(ILogger<UserController> logger, IUserServices userServices)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] UsuarioNuevo newuser)
        {
            try
            {
                var responseUser = await _userServices.CreatedUser(newuser);

                if (responseUser.Status)
                {
                    var response = new
                    {
                        estatus = true,
                        datos = new
                        {
                            key_session = responseUser.KeySession,
                            id = responseUser.Id,
                            rut = newuser.Rut,
                            digVer = newuser.DigVer,
                            mail = newuser.Mail,
                            nombres = newuser.Nombres,
                            apellidos = newuser.Apellidos,
                            telefono = newuser.Telefono
                        }
                    };

                    return Ok(response);
                }

                var responseHttp = new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        message = responseUser.Message
                    }
                };

                return BadRequest(responseUser);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"error al crear usuario");

                return StatusCode(500, "Error de servidor");
            }

        }
    }
}
