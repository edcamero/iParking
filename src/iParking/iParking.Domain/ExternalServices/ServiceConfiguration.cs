

namespace iParking.Domain.ExternalServices
{
    public class ServiceConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public SecurityConfiguration Security { get; set; } = null!;
    }
}
