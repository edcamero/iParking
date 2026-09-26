using iParking.Application.ServicesExternal;
using iParking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iParking.API.Controllers
{
    /// <summary>
    /// Controlador para gestión de pagos mediante pasarelas externas.
    /// IMPORTANTE: Este controlador NUNCA recibe datos sensibles de tarjetas de crédito.
    /// Los pagos se procesan mediante redirección a URLs seguras de proveedores externos.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentGatewayService paymentGatewayService,
            ILogger<PaymentController> logger)
        {
            _paymentGatewayService = paymentGatewayService ?? throw new ArgumentNullException(nameof(paymentGatewayService));
            _logger = logger;
        }

        /// <summary>
        /// Inicia un proceso de pago redirigiendo a la pasarela externa segura.
        /// </summary>
        /// <param name="paymentData">Datos básicos del pago (sin información sensible de tarjetas)</param>
        /// <returns>URL de redirección para completar el pago en la pasarela externa</returns>
        [HttpPost("initiate")]
        [Authorize]
        [ProducesResponseType(typeof(ResponsePayDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponsePayDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InitiatePayment([FromBody] RequestPayExternal paymentData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponsePayDTO(false, "Datos de pago inválidos"));
                }

                var responsePay = await _paymentGatewayService.InitiatePaymentAsync(paymentData);

                if (responsePay.Status)
                {
                    _logger.LogInformation("Pago iniciado exitosamente. Referencia: {ReferenceId}", paymentData.reference_id);
                    return Ok(responsePay);
                }

                _logger.LogWarning("Error al iniciar pago: {Message}", responsePay.Data.ErrorMessage);
                return BadRequest(responsePay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error excepcional al iniciar pago");
                return StatusCode(500, new ResponsePayDTO(false, "Error interno del servidor. Por favor intente nuevamente."));
            }
        }

        /// <summary>
        /// Consulta el estado de un pago usando el ID de transacción externo.
        /// </summary>
        /// <param name="transactionId">ID de transacción proporcionado por la pasarela externa</param>
        [HttpGet("status/{transactionId}")]
        [Authorize]
        [ProducesResponseType(typeof(ResponsePayDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponsePayDTO), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentStatus(string transactionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(transactionId))
                {
                    return BadRequest(new ResponsePayDTO(false, "ID de transacción requerido"));
                }

                var responsePay = await _paymentGatewayService.GetPaymentStatusAsync(transactionId);

                if (responsePay.Status)
                {
                    return Ok(responsePay);
                }

                return NotFound(responsePay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar estado del pago: {TransactionId}", transactionId);
                return StatusCode(500, new ResponsePayDTO(false, "Error al consultar el estado del pago"));
            }
        }

        /// <summary>
        /// Procesa un reembolso parcial o total mediante la pasarela externa.
        /// </summary>
        /// <param name="transactionId">ID de transacción original</param>
        /// <param name="amount">Monto a reembolsar</param>
        [HttpPost("refund/{transactionId}")]
        [Authorize(Roles = "SuperAdmin,CompanyAdmin")]
        [ProducesResponseType(typeof(ResponsePayDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponsePayDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessRefund(string transactionId, [FromQuery] decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(transactionId))
                {
                    return BadRequest(new ResponsePayDTO(false, "ID de transacción requerido"));
                }

                if (amount <= 0)
                {
                    return BadRequest(new ResponsePayDTO(false, "El monto del reembolso debe ser mayor a cero"));
                }

                var responsePay = await _paymentGatewayService.ProcessRefundAsync(transactionId, amount);

                if (responsePay.Status)
                {
                    _logger.LogInformation("Reembolso procesado. Transacción: {TransactionId}, Monto: {Amount}", transactionId, amount);
                    return Ok(responsePay);
                }

                return BadRequest(responsePay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar reembolso: {TransactionId}", transactionId);
                return StatusCode(500, new ResponsePayDTO(false, "Error al procesar el reembolso"));
            }
        }
    }
}
