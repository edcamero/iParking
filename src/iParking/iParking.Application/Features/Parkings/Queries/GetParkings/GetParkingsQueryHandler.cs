using MediatR;
using iParking.DataAccess.DataServices;

namespace iParking.Application.Features.Parkings.Queries.GetParkings
{
    public class GetParkingsQueryHandler : IRequestHandler<GetParkingsQuery, List<DataAccess.Models.Parking>>
    {
        private readonly IParkingDataServices _parkingDataServices;

        public GetParkingsQueryHandler(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }

        public async Task<List<DataAccess.Models.Parking>> Handle(GetParkingsQuery request, CancellationToken cancellationToken)
        {
            return await _parkingDataServices.GetParkings();
        }
    }
}
