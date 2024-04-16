namespace iParking.Domain.ParkingModels
{
    public class NearbyParkingLot
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Tariff { get; set; } = null!;
        public string Schedule { get; set; } = null!;
        public string Addresss { get; set; } = "sin dirección";
        public int AvailablePlaces { get; set; }
        public bool State { get; set; }
        public double DistanceKm { get; set; }
    }
}
