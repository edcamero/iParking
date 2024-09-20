using iParking.Domain.ExternalServices.RequestEntities;
using Newtonsoft.Json;
using System.Text;

namespace iParking.Infrastructure.Services
{
    public class IntegrationServiceClient: IIntegrationServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IntegrationServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));        
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, RequestGenerateOrder requestData)
        {

            var request = new HttpRequestMessage(httpMethod, requestUrl);

            // Agregar datos de la solicitud
            var jsonContent = JsonConvert.SerializeObject(requestData);

            // Agregar los datos serializados al cuerpo de la solicitud
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
           // var httpClient = new HttpClient();

            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, RequestGenerateOrder requestData, Dictionary<string, string> headers )
        {

            var request = new HttpRequestMessage(httpMethod, requestUrl);

            // Agregar datos de la solicitud
            var jsonContent = JsonConvert.SerializeObject(requestData);

            // Agregar los datos serializados al cuerpo de la solicitud
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
            // var httpClient = new HttpClient();

            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, Dictionary<string, string> headers)
        {

            var request = new HttpRequestMessage(httpMethod, requestUrl);

            var httpClient = _httpClientFactory.CreateClient();

            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return await httpClient.SendAsync(request);
        }
    }
}
