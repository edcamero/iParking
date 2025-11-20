namespace iParking.Domain.Entities.Vehicle
{
    public class EVehicle
    {
        public decimal IdPlaca { get; set; }
        public string Placa { get; set; }
        public decimal? PlacaDefault { get; set; }
        public decimal? VehicleOwnerId { get; set; }
        public string FechaHoraCreado { get; set; }
        public decimal? Estado { get; set; }
        public int? TenantId { get; set; }
    }
}
