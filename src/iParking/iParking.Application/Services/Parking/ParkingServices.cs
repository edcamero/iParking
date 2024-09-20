using iParking.DataAccess.DataServices;
using iParking.Domain.Parking;
using iParking.Domain.ParkingModels;
using ParkingData = iParking.DataAccess.Models.Parking;


namespace iParking.Application.Services.Parking
{
    public class ParkingServices: IParkingServices
    {
        private readonly IParkingDataServices _parkingDataServices;

        public ParkingServices(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }


        public Task<ParkingData> CreateParking(ParkingInputDTO parkingInput)
        {
            DateTime currentDate = DateTime.Now;

            var parkingRepository = new DataAccess.Models.Parking
            {
                Location = parkingInput.Location,
                Name = parkingInput.Name,
                CreatedAt = currentDate,
                UpdatedAt = currentDate,
            };

            return _parkingDataServices.CreateParking(parkingRepository);
        }

        public async Task<ParkingData> GetParking(int id)
        {
            return await _parkingDataServices.GetParking(id);
        }

        public async Task<List<ParkingData>> GetParkings()
        {
            return await _parkingDataServices.GetParkings();
        }

        public async Task<List<NearbyParkingLot>> GetNearbyParkings(NearbyParkingLotInput input)
        {
            var parkings =  await _parkingDataServices.GetNearbyParkings();

            foreach (NearbyParkingLot parking in parkings)
            {
                parking.DistanceKm = CalculateHaversineDistance(
                    input.MyLatitude, input.MyLongitude,
                    parking.Latitude, parking.Longitude);         
            }

            return parkings.Where( parking => parking.DistanceKm <= input.RadiusInkilometer).ToList();

        }

        public Task<ParkingData?> UpdateParking(ParkingInputUpdateDTO parkingInput, int id)
        {
            DateTime currentDate = DateTime.Now;

            var parkingRepository = new DataAccess.Models.Parking
            {
                Id = id,
                Location = parkingInput.Location,
                Name = parkingInput.Name,
                UpdatedAt = currentDate,
            };

            return _parkingDataServices.UpdateParking(parkingRepository);
        }

        public async Task<ParkingData?> DeleteParking(int id)
        {
            return await _parkingDataServices.DeleteParking(id);
        }

        public static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radio de la Tierra en kilómetros
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
