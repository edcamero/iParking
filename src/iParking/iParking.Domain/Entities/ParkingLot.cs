namespace iParking.Domain.Entities
{
    public class ParkingLot
    {
        public int ParkingLotId { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string OpeningHours { get; set; }
        public bool IsActive { get; set; }
    }
}
