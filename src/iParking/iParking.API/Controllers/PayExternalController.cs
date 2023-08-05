using Azure;
using iParking.Application;
using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;
using Microsoft.AspNetCore.Mvc;

namespace iParking.API.Controllers
{
    [ApiController]
    public class PayExternalController : ControllerBase
    {
        private readonly IPayExtenalService _payExtenalService;
        public PayExternalController(IPayExtenalService payExtenalService)
        {
            _payExtenalService = payExtenalService ?? throw new ArgumentNullException(nameof(payExtenalService));
        }

        [HttpPost]
        [Route("api/v1/payment")]
        public async Task<IActionResult> MakePayment([FromForm] RequestPayExternal paymentData)
        {
            try
            {
                var reponsePay = await _payExtenalService.MakePaymentPost(paymentData);

                if (reponsePay.Status)
                {
                    return Ok(reponsePay);
                }

                return BadRequest(reponsePay);
            }
            catch
            {
                return StatusCode(500, "Error de servidor");
            }         
        }

        [HttpPost]
        [Route("api/v1/payment/methods")]
        public async Task<IActionResult> GetPaymentMethods([FromForm] FormasPagosRequest request)
        {
            if (request.KeySession == "123456789012345")
            {

                var response = new
                {
                    estatus = true,
                    datos = new
                    {
                        formasPago = await _payExtenalService.GetPaymentMethods()
                    }
                };

                return Ok(response);
            }
            else
            {
                return BadRequest(new
                {
                    estatus = false,
                    datos = new
                    {
                        key_session = "0",
                        mensajeError = "Usuario o clave Invalida!"
                    }
                });
            }
        }
    }
}
