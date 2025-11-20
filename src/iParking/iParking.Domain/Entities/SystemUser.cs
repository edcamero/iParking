namespace iParking.Domain.Entities
{
    public class SystemUser
    {
        public int SystemUserId { get; set; }
        public int? TenantId { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
