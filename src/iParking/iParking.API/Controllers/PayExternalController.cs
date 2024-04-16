using iParking.Application.ServicesExternal;
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
            catch(Exception ex) 
            {
                return StatusCode(500, "Error de servidor");
            }
        }

        [HttpGet]
        [Route("api/v1/payment")]
        public async Task<IActionResult> GetPayment(string order_id)
        {
            try
            {
                var reponsePay = await _payExtenalService.GetPayment(order_id);

                if (reponsePay.Status)
                {
                    return Ok(reponsePay);
                }

                return BadRequest(reponsePay);
            }
            catch (Exception ex)
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
                    status = true,
                    data = new
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
                    status = false,
                    data = new
                    {
                        keySession = "0",
                        mensajeError = "Usuario o clave Invalida!"
                    }
                });
            }
        }

        [HttpPost]
        [Route("api/v1/payment/detail")]
        public IActionResult GetPaymentDetail([FromForm] PayDetails request)
        {

            string entryTime = "10:10";
            string exitTime = "10:20";
            string stayDuration = "00:10";
            string amount = "150";
            string location = "PZA LOS HEROES 1";

            var result = new
            {
                status = true,
                data = new
                {
                    Entry = entryTime,
                    Exit = exitTime,
                    StayDuration = stayDuration,
                    Amount = amount,
                    Location = location
                }
            };


            return Ok(result);

            //else
            //{
            //    var errorResult = new
            //    {
            //        status = false,
            //        data = new
            //        {
            //            errorMessage = "Error!"
            //        }
            //    };

            //    return Ok(errorResult);
            //}
        }
    }
}
