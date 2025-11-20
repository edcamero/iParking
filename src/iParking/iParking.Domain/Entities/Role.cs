namespace iParking.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; } // Nullable for system-wide roles
    }
}
