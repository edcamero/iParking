using MediatR;
using iParking.Domain.ParkingModels;

namespace iParking.Application.Features.Parkings.Queries.GetNearbyParkings
{
    public class GetNearbyParkingsQuery : IRequest<List<NearbyParkingLot>>
    {
        public double MyLatitude { get; set; }
        public double MyLongitude { get; set; }
        public double RadiusInKilometer { get; set; }
    }
}
