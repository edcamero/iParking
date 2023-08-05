
using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;
using iParking.Infrastructure.Identity;
using iParking.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace iParking.Application
{
    public class PayExtenalService : IPayExtenalService
    {
        private readonly MyServiceConfiguration _configuration;
        private readonly IIntegrationServiceClient _integrationServiceClient;

        public PayExtenalService(IConfiguration configuration, IIntegrationServiceClient integrationServiceClient)
        {
            _configuration = new MyServiceConfiguration();
            _configuration.Url = configuration["MyServiceConfiguration:Url"] ?? throw new ArgumentNullException(nameof(configuration));

            // configuration.GetSection("MyServiceConfiguration").Bind(_configuration);
            _integrationServiceClient = integrationServiceClient ?? throw new ArgumentNullException(nameof(integrationServiceClient)); ;
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


            var response = await _integrationServiceClient.SendRequestAsync(_configuration.Url, HttpMethod.Post, paymentDataDictionary);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                ResponsePayKlap? responsePay = JsonConvert.DeserializeObject<ResponsePayKlap>(responseBody);

                if ((responsePay is null))
                {
                    return new ResponsePayDTO(false, "Algo salio mal, por favor intente contactarse con su proveedor");
                }

                return new ResponsePayDTO(true, responsePay.Result.RedirectUrl.ToString());
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
