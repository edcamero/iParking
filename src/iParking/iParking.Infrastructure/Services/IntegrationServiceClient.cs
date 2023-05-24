namespace iParking.Infrastructure.Services
{
    public class IntegrationServiceClient: IIntegrationServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IntegrationServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));        
        }

        public async Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, Dictionary<string, string> requestData)
        {

            var request = new HttpRequestMessage(httpMethod, requestUrl);

            // Agregar datos de la solicitud
            request.Content = new FormUrlEncodedContent(requestData);
            var httpClient = _httpClientFactory.CreateClient();
           // var httpClient = new HttpClient();

            return await httpClient.SendAsync(request);
        }
    }
}
