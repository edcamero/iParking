using MediatR;
using iParking.DataAccess.DataServices;

namespace iParking.Application.Features.Parkings.Queries.GetParking
{
    public class GetParkingQueryHandler : IRequestHandler<GetParkingQuery, DataAccess.Models.Parking>
    {
        private readonly IParkingDataServices _parkingDataServices;

        public GetParkingQueryHandler(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }

        public async Task<DataAccess.Models.Parking> Handle(GetParkingQuery request, CancellationToken cancellationToken)
        {
            return await _parkingDataServices.GetParking(request.Id);
        }
    }
}
