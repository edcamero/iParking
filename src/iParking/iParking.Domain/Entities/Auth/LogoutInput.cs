namespace iParking.Domain.Entities.Auth
{
    public class LogoutInput
    {
        public string KeySession { get; set; }
        public string Mail { get; set; }
        public string ClaveAcceso { get; set; }
        public string ImeiPos { get; set; }
        public string SerieCelular { get; set; }
        public string VersionApp { get; set; }
    }

}
