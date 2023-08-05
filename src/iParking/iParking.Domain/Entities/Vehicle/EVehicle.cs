namespace iParking.Domain.Entities.Vehicle
{
    public class EVehicle
    {
        public decimal IdPlaca { get; set; }
        public string Placa { get; set; }
        public decimal? PlacaDefault { get; set; }
        public decimal? IdUsuario { get; set; }
        public string FechaHoraCreado { get; set; }
        public decimal? Estado { get; set; }
    }
}
