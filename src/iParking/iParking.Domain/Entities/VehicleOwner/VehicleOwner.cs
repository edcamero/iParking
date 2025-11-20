namespace iParking.Domain.Entities.VehicleOwner
{
    public class VehicleOwner
    {
        public int IdUsuario { get; set; }
        public string Rut { get; set; }
        public string Dv { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string ClaveAcceso { get; set; }
        public string Mail { get; set; }
        public int Estado { get; set; }
        public int? TenantId { get; set; }
    }

}
