using System;

namespace iParking.Domain.Entities
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
