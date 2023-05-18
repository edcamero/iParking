using iParking.Application;
using iParking.Domain.Entities;
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
            var reponsePay = await _payExtenalService.MakePaymentPost(paymentData);

            return Ok(reponsePay);
        }
    }
}
