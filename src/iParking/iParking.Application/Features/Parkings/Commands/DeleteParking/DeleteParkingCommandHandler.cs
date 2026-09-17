using MediatR;
using iParking.DataAccess.DataServices;

namespace iParking.Application.Features.Parkings.Commands.DeleteParking
{
    public class DeleteParkingCommandHandler : IRequestHandler<DeleteParkingCommand, DataAccess.Models.Parking?>
    {
        private readonly IParkingDataServices _parkingDataServices;

        public DeleteParkingCommandHandler(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }

        public async Task<DataAccess.Models.Parking?> Handle(DeleteParkingCommand request, CancellationToken cancellationToken)
        {
            return await _parkingDataServices.DeleteParking(request.Id);
        }
    }
}
