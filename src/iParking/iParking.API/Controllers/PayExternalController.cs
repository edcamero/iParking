using iParking.Application;
using iParking.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

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
    }
}
