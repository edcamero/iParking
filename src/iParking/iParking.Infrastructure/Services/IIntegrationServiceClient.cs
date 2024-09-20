using iParking.Domain.ExternalServices.RequestEntities;

namespace iParking.Infrastructure.Services
{
    public  interface IIntegrationServiceClient
    {
        Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, RequestGenerateOrder requestData);
        Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, RequestGenerateOrder requestData, Dictionary<string, string> headers);
        Task<HttpResponseMessage> SendRequestAsync(string requestUrl, HttpMethod httpMethod, Dictionary<string, string> headers);
    }
}
