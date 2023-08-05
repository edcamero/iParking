using Azure;
using iParking.Application.Services.CreditCard;
using iParking.Domain.Entities.Payment;
using Microsoft.AspNetCore.Mvc;

namespace iParking.API.Controllers
{
    [Route("api/v1/creditcard")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ICreditCardServices _creditCardServices;
        public CreditCardController(ILogger<UserController> logger, ICreditCardServices creditCardServices)
        {
            _logger = logger;
            _creditCardServices = creditCardServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreatedCreditCard(CreditCardInput creditCard)
        {
            try
            {
                var response = await _creditCardServices.CreatedCreditCard(creditCard);

                if (response.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        data = new
                        {
                            keySession = "123456789012345",
                            id = response.Id,
                        }
                    });

                }

                return BadRequest(new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        errorMessage = response.Message
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al crear tarjeta");

                return StatusCode(500, "Error de servidor");
            }
        }

        [HttpDelete]
        [Route("{userId}/delete/{cardId}")]
        public async Task<IActionResult> DeleteCreditCard(int userId, int creditCardId)
        {
            try
            {
                var response = await _creditCardServices.DeleteCreditCard(userId, creditCardId);

                if (response.Status)
                {
                    return Ok(new
                    {
                        status = true,
                        data = new
                        {
                            keySession = "123456789012345",
                            id = response.Id,
                        }
                    });
                }

                return BadRequest(new
                {
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        errorMessage = response.Message
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al eliminar tarjeta");

                return StatusCode(500, "Error de servidor");
            }
        }

        [Route("{userId}/default/{cardId}")]
        [HttpPut]
        public async Task<IActionResult> UpdatedUserVehicleDefault(int userId, int cardId)
        {
            try
            {
                var vehicleResponse = await _creditCardServices.UpdatedCreditCardDefault(userId, cardId);

                if (vehicleResponse.Status)
                {
                    var response = new
                    {
                        estatus = true,
                        datos = new
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
                _logger.LogError(ex, "error al actualizar la tarjeta");

                return StatusCode(500, "Error de servidor");
            }
        }
    }
}
