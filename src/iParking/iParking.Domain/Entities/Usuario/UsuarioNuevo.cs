namespace iParking.Domain.Entities.Usuario
{
    public  class UsuarioNuevo
    {
        public string Rut { get; set; }
        public string DigVer { get; set; }
        public string Mail { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string ClaveAcceso { get; set; }
        public string ImeiCelular { get; set; }
        public string SerieCelular { get; set; }
        public string VersionApp { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
    }
}
