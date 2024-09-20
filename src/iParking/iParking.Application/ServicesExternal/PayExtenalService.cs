using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;
using iParking.Domain.ExternalServices;
using iParking.Domain.ExternalServices.RequestEntities;
using iParking.Infrastructure.Services;
using Newtonsoft.Json;

namespace iParking.Application.ServicesExternal
{
    public class PayExtenalService : IPayExtenalService
    {
   
        private readonly IIntegrationServiceClient _integrationServiceClient;
        private readonly ServiceConfiguration KlapService;
        private readonly ServiceConfiguration PHPService;

        public PayExtenalService( IIntegrationServiceClient integrationServiceClient,IServicesConfigurationService servicesConfigurationService)
        {
            _integrationServiceClient = integrationServiceClient ?? throw new ArgumentNullException(nameof(integrationServiceClient));

            KlapService = servicesConfigurationService.GetServiceConfiguration(ServiceType.Klap);
            PHPService = servicesConfigurationService.GetServiceConfiguration(ServiceType.PHP);
        }

        public async Task<ResponsePayDTO> MakePaymentPost(RequestPayExternal paymentData)
        {

            Dictionary<string, string> paymentDataDictionary = new Dictionary<string, string>
            {
                { "reference_id", paymentData.reference_id },
                { "email", paymentData.email },
                { "rut", paymentData.rut },
                { "phone", paymentData.phone },
                { "first_name", paymentData.first_name },
                { "last_name", paymentData.last_name },
                { "address_line", paymentData.address_line },
                { "address_city", paymentData.address_city },
                { "address_state", paymentData.address_state },
                { "amount", paymentData.amount },
                { "name_item", paymentData.name_item },
                { "code_item", paymentData.code_item }
            };

            var requestPayment = RequestGenerateOrderMethods.CreateRequest(paymentData);

            var requestURl = string.Format("{0}/payment-gateway/v1/orders", KlapService.Url);

            var headers = new Dictionary<string, string>
            {
                { KlapService.Security.Type, KlapService.Security.Value }
            };

            var response = await _integrationServiceClient.SendRequestAsync(requestURl, HttpMethod.Post, requestPayment, headers);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                ResponseGenerateOrder? responsePay = JsonConvert.DeserializeObject<ResponseGenerateOrder>(responseBody);
                //ResponsePayKlap? responsePay = JsonConvert.DeserializeObject<ResponsePayKlap>(responseBody);

                if (responsePay is null)
                {
                    return new ResponsePayDTO(false, "Algo salio mal, por favor intente contactarse con su proveedor");
                }

                //return new ResponsePayDTO(true, responsePay.Result.RedirectUrl.ToString());
                return new ResponsePayDTO(true, responsePay.RedirectUrl.ToString());
            }

            return new ResponsePayDTO(false, "Algo salio mal, por favor intente contactarse con su proveedor");
        }

        public async Task<ResponsePayDTO> GetPayment(string orderId)
        {

            var requestURl = string.Format("{0}/payment-gateway/v1/orders/{1}", KlapService.Url, orderId);

            var headers = new Dictionary<string, string>
            {
                { KlapService.Security.Type, KlapService.Security.Value }
            };

            var response = await _integrationServiceClient.SendRequestAsync(requestURl, HttpMethod.Post, headers);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                ResponseGenerateOrder? responsePay = JsonConvert.DeserializeObject<ResponseGenerateOrder>(responseBody);

                if (responsePay is null)
                {
                    return new ResponsePayDTO(false, "Algo salio mal, por favor intente contactarse con su proveedor");
                }

                return new ResponsePayDTO(true, responsePay.Status);
            }

            return new ResponsePayDTO(false, "Algo salio mal, por favor intente contactarse con su proveedor");
        }

        public async Task<List<FormaPago>> GetPaymentMethods()
        {
            var formasDePago = new List<FormaPago>();

            formasDePago.Add(new FormaPago(1, "1234", true, "VISA"));
            formasDePago.Add(new FormaPago(2, "4557", false, "MASTER"));
            formasDePago.Add(new FormaPago(3, "2345", true, "DINNER"));

            return await Task.FromResult(formasDePago);
        }
    }
}
