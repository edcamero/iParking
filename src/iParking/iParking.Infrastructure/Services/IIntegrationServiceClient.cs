namespace iParking.Infrastructure.Services
{
    public  interface IIntegrationServiceClient
    {
          Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, Dictionary<string, string> requestData);
    }
}
