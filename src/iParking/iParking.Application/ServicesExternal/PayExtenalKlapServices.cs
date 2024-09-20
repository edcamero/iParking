using iParking.Domain.ExternalServices;
using iParking.Infrastructure.Services;

namespace iParking.Application.ServicesExternal
{
    public class PayExtenalKlapServices
    {
        private readonly IIntegrationServiceClient _integrationServiceClient;
        private readonly ServiceConfiguration serviceConfiguration;

        public PayExtenalKlapServices(IIntegrationServiceClient integrationServiceClient, IServicesConfigurationService servicesConfigurationService)
        {
            _integrationServiceClient = integrationServiceClient ?? throw new ArgumentNullException(nameof(integrationServiceClient));
            serviceConfiguration = servicesConfigurationService.GetServiceConfiguration(ServiceType.Klap);
        }
    }
}
