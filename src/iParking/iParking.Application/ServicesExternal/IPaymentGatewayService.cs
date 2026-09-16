using iParking.Domain.Entities;

namespace iParking.Application.ServicesExternal
{
    /// <summary>
    /// Servicio para integración con pasarelas de pago externas (Stripe, PayPal, ePayco, etc.)
    /// IMPORTANTE: Este servicio NUNCA maneja datos sensibles de tarjetas de crédito.
    /// Solo trabaja con tokens y IDs de transacción proporcionados por el proveedor externo.
    /// </summary>
    public interface IPaymentGatewayService
    {
        /// <summary>
        /// Inicia un proceso de pago con la pasarela externa
        /// </summary>
        Task<ResponsePayDTO> InitiatePaymentAsync(RequestPayExternal paymentData);
        
        /// <summary>
        /// Consulta el estado de un pago usando el ID de transacción externo
        /// </summary>
        Task<ResponsePayDTO> GetPaymentStatusAsync(string externalTransactionId);
        
        /// <summary>
        /// Procesa un reembolso mediante la pasarela externa
        /// </summary>
        Task<ResponsePayDTO> ProcessRefundAsync(string externalTransactionId, decimal amount);
    }
}
