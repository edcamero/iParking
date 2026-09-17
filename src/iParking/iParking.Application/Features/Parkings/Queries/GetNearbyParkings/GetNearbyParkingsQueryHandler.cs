using MediatR;
using iParking.DataAccess.DataServices;
using iParking.Domain.ParkingModels;

namespace iParking.Application.Features.Parkings.Queries.GetNearbyParkings
{
    public class GetNearbyParkingsQueryHandler : IRequestHandler<GetNearbyParkingsQuery, List<NearbyParkingLot>>
    {
        private readonly IParkingDataServices _parkingDataServices;

        public GetNearbyParkingsQueryHandler(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }

        public async Task<List<NearbyParkingLot>> Handle(GetNearbyParkingsQuery request, CancellationToken cancellationToken)
        {
            var parkings = await _parkingDataServices.GetNearbyParkings();

            foreach (NearbyParkingLot parking in parkings)
            {
                parking.DistanceKm = CalculateHaversineDistance(
                    request.MyLatitude, request.MyLongitude,
                    parking.Latitude, parking.Longitude);
            }

            return parkings.Where(parking => parking.DistanceKm <= request.RadiusInKilometer).ToList();
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
