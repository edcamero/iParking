namespace iParking.Domain.Entities.Usuario
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? Rut { get; set; }
        public string Dv { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string ClaveAcceso { get; set; }
        public string Mail { get; set; }
        public int Estado { get; set; }
    }

}
