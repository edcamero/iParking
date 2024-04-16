using iParking.Application.Services.Vehicle;
using iParking.Domain.Entities.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace iParking.API.Controllers
{
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IVehicleServices _vehicleServices;

        public VehicleController(ILogger<UserController> logger, IVehicleServices vehicleServices)
        {
            _logger = logger;
            _vehicleServices = vehicleServices;
        }


        [Route("api/v1/user/{userId}/vehicles")]
        [HttpGet]
        public async Task<IActionResult> GetUserVehicles(int userId)
        {
            try
            {
                var (vehicles, reponse) = await _vehicleServices.GetUserVehicles(userId);

                if (reponse.Status)
                {
                    var responseCustom = new
                    {
                        status = true,
                        data = new
                        {
                            ListadoPlacas = vehicles
                        }
                    };

                    return Ok(responseCustom);
                }

                return NotFound(new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        errorMessage = reponse.Message
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al obtener los vehiculos del usuario");

                return StatusCode(500, "Error de servidor");
            }
        }

        [Route("api/v1/user/{userId}/vehicles")]
        [HttpPost]
        public async Task<IActionResult> CreateUserVehicle([FromForm] VehicleUserInput newuser)
        {
            try
            {
                var (vehicledSaved,vehicleResponse) = await _vehicleServices.CreatedUserVehicle(newuser);

                if (vehicleResponse.Status)
                {
                    var response = new
                    {
                        status = true,
                        data = vehicledSaved
                    };

                    return Ok(response);
                }

                var responseHttp = new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        message = vehicleResponse.Message
                    }
                };

                return BadRequest(vehicleResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al crear el vehiculo");

                return StatusCode(500, "Error de servidor");
            }
        }

        [Route("api/v1/user/{userId}/vehicles/{vehicleId}/default")]
        [HttpPut]
        public async Task<IActionResult> UpdatedUserVehicleDefault(int userId, int vehicleId)
        {
            try
            {
                var vehicleResponse = await _vehicleServices.UpdatedUserVehicleDefault(userId, vehicleId);

                if (vehicleResponse.Status)
                {
                    var response = new
                    {
                        status = true,
                        data = new
                        {
                            keySession = vehicleResponse.KeySession,
                            id = vehicleResponse.Id
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
                        message = vehicleResponse.Message
                    }
                };

                return BadRequest(vehicleResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al actualizar el vehiculo");

                return StatusCode(500, "Error de servidor");
            }
        }

        [Route("api/v1/user/{userId}/vehicles/{vehicleId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserVehicle(int userId, int vehicleId)
        {
            try
            {
                var vehicleResponse = await _vehicleServices.DeletedUserVehicle(userId, vehicleId);

                if (vehicleResponse.Status)
                {
                    var response = new
                    {
                        status = true,
                        data = new
                        {
                            keysession = vehicleResponse.KeySession,
                            id = vehicleResponse.Id
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
                        message = vehicleResponse.Message
                    }
                };

                return BadRequest(vehicleResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al eliminar el vehiculo");

                return StatusCode(500, "Error de servidor");
            }
        }
    }
}
