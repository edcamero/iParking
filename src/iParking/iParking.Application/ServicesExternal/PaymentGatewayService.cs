using iParking.Domain.Entities;
using iParking.Domain.ExternalServices;
using iParking.Domain.ExternalServices.RequestEntities;
using iParking.Infrastructure.Services;
using Newtonsoft.Json;

namespace iParking.Application.ServicesExternal
{
    /// <summary>
    /// Implementación de servicio de pasarela de pagos externa (Klap en este caso)
    /// IMPORTANTE: Este servicio NUNCA almacena ni procesa datos sensibles de tarjetas.
    /// Todos los datos sensibles se manejan directamente por la pasarela externa.
    /// </summary>
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly IIntegrationServiceClient _integrationServiceClient;
        private readonly ServiceConfiguration _paymentGatewayService;

        public PaymentGatewayService(
            IIntegrationServiceClient integrationServiceClient,
            IServicesConfigurationService servicesConfigurationService)
        {
            _integrationServiceClient = integrationServiceClient ?? throw new ArgumentNullException(nameof(integrationServiceClient));
            _paymentGatewayService = servicesConfigurationService.GetServiceConfiguration(ServiceType.Klap);
        }

        /// <inheritdoc/>
        public async Task<ResponsePayDTO> InitiatePaymentAsync(RequestPayExternal paymentData)
        {
            var requestPayment = RequestGenerateOrderMethods.CreateRequest(paymentData);

            var requestUrl = $"{_paymentGatewayService.Url}/payment-gateway/v1/orders";

            var headers = new Dictionary<string, string>
            {
                { _paymentGatewayService.Security.Type, _paymentGatewayService.Security.Value }
            };

            var response = await _integrationServiceClient.SendRequestAsync(
                requestUrl, 
                HttpMethod.Post, 
                requestPayment, 
                headers);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var responsePay = JsonConvert.DeserializeObject<ResponseGenerateOrder>(responseBody);

                if (responsePay is null)
                {
                    return new ResponsePayDTO(
                        false, 
                        "Error al procesar la respuesta del proveedor de pagos. Por favor intente nuevamente.");
                }

                // Retorna URL de redirección para completar el pago en la pasarela segura
                return new ResponsePayDTO(true, responsePay.RedirectUrl.ToString());
            }

            return new ResponsePayDTO(
                false, 
                "Error al conectar con la pasarela de pagos. Verifique su conexión o contacte al administrador.");
        }

        /// <inheritdoc/>
        public async Task<ResponsePayDTO> GetPaymentStatusAsync(string externalTransactionId)
        {
            var requestUrl = $"{_paymentGatewayService.Url}/payment-gateway/v1/orders/{externalTransactionId}";

            var headers = new Dictionary<string, string>
            {
                { _paymentGatewayService.Security.Type, _paymentGatewayService.Security.Value }
            };

            var response = await _integrationServiceClient.SendRequestAsync(
                requestUrl, 
                HttpMethod.Get, 
                headers);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var responsePay = JsonConvert.DeserializeObject<ResponseGenerateOrder>(responseBody);

                if (responsePay is null)
                {
                    return new ResponsePayDTO(
                        false, 
                        "Error al obtener el estado del pago. Por favor verifique el ID de transacción.");
                }

                return new ResponsePayDTO(true, responsePay.Status);
            }

            return new ResponsePayDTO(
                false, 
                "Error al consultar el estado del pago. Intente nuevamente.");
        }

        /// <inheritdoc/>
        public async Task<ResponsePayDTO> ProcessRefundAsync(string externalTransactionId, decimal amount)
        {
            // TODO: Implementar lógica de reembolso según API de Klap
            // Esto depende de las capacidades específicas de la pasarela
            
            var requestUrl = $"{_paymentGatewayService.Url}/payment-gateway/v1/refunds";

            var refundRequest = new
            {
                order_id = externalTransactionId,
                amount = amount.ToString("F2"),
                reason = "Reembolso solicitado"
            };

            var headers = new Dictionary<string, string>
            {
                { _paymentGatewayService.Security.Type, _paymentGatewayService.Security.Value }
            };

            var response = await _integrationServiceClient.SendRequestAsync(
                requestUrl, 
                HttpMethod.Post, 
                refundRequest, 
                headers);

            if (response.IsSuccessStatusCode)
            {
                return new ResponsePayDTO(true, "Reembolso procesado exitosamente");
            }

            return new ResponsePayDTO(false, "Error al procesar el reembolso. Contacte al proveedor de pagos.");
        }
    }
}
