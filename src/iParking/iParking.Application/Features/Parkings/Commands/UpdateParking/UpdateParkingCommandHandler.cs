using MediatR;
using iParking.DataAccess.DataServices;

namespace iParking.Application.Features.Parkings.Commands.UpdateParking
{
    public class UpdateParkingCommandHandler : IRequestHandler<UpdateParkingCommand, DataAccess.Models.Parking?>
    {
        private readonly IParkingDataServices _parkingDataServices;

        public UpdateParkingCommandHandler(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }

        public async Task<DataAccess.Models.Parking?> Handle(UpdateParkingCommand request, CancellationToken cancellationToken)
        {
            DateTime currentDate = DateTime.Now;

            var parking = new DataAccess.Models.Parking
            {
                Id = request.Id,
                Location = request.Location,
                Name = request.Name,
                UpdatedAt = currentDate,
            };

            return await _parkingDataServices.UpdateParking(parking);
        }
    }
}
