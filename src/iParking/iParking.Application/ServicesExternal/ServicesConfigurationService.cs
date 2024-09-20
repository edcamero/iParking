using iParking.Domain.ExternalServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace iParking.Application.ServicesExternal
{
    public class ServicesConfigurationService : IServicesConfigurationService
    {
        private readonly List<ServiceConfiguration> _servicesConfiguration;

        public ServicesConfigurationService(IConfiguration configuration)
        {
            _servicesConfiguration = configuration.GetSection("ServicesConfiguration").Get<List<ServiceConfiguration>>() ?? throw new ArgumentNullException("ServicesConfiguration:supplier");


            //var servicesConfigurationList = JsonConvert.DeserializeObject<ServicesConfigurationList>(servicesConfigurationJson ?? throw new ArgumentNullException(nameof(servicesConfigurationJson)));

            //if (servicesConfigurationList is null)
            //{
            //    throw new ArgumentNullException(nameof(servicesConfigurationList));
            //}
            //_servicesConfiguration = servicesConfigurationList.Services;
        }

        public ServiceConfiguration GetServiceConfiguration(ServiceType id)
        {
            return _servicesConfiguration.First(service => service.Id == ((int)id));
        }
    }

    public class ServicesConfigurationList
    {
        public List<ServiceConfiguration> Services { get; set; } = null!;
    }
}
