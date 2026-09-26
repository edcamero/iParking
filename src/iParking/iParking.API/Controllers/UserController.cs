using iParking.Application.Services.User;
using iParking.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using iParking.Domain.Shared;

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
        public async Task<IActionResult> CreateUser([FromForm] CreateUserDto newUser)
        {
            try
            {
                var responseUser = await _userServices.CreatedUser(newUser);

                if (responseUser.Status)
                {
                    var user = await _userServices.GetUser(newUser.Email);

                    var response = new ApiResponse<object>
                    {
                        Status = true,
                        Data = new
                        {
                            keySession = responseUser.KeySession,
                            id = responseUser.Id,
                            rut = newUser.Rut,
                            dv = newUser.Dv,
                            email = newUser.Email,
                            fullName = $"{newUser.FirstName} {newUser.LastName}",
                            phone = newUser.PhoneNumber
                        }
                    };

                    return Ok(response);
                }

                return BadRequest(ApiResponse<object>.Failure(responseUser.Message));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"error al crear usuario");
                return StatusCode(500, ApiResponse<object>.Failure("Error de servidor"));
            }
        }
    }
}
