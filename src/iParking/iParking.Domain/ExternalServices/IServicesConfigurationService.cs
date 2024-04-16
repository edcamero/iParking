namespace iParking.Domain.ExternalServices
{
    public interface IServicesConfigurationService
    {
        ServiceConfiguration GetServiceConfiguration(ServiceType id);
    }
}
