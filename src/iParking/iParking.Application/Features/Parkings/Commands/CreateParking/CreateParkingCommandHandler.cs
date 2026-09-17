using MediatR;
using iParking.DataAccess.DataServices;
using iParking.Domain.ParkingModels;

namespace iParking.Application.Features.Parkings.Commands.CreateParking
{
    public class CreateParkingCommandHandler : IRequestHandler<CreateParkingCommand, DataAccess.Models.Parking>
    {
        private readonly IParkingDataServices _parkingDataServices;

        public CreateParkingCommandHandler(IParkingDataServices parkingDataServices)
        {
            _parkingDataServices = parkingDataServices ?? throw new ArgumentNullException(nameof(parkingDataServices));
        }

        public async Task<DataAccess.Models.Parking> Handle(CreateParkingCommand request, CancellationToken cancellationToken)
        {
            DateTime currentDate = DateTime.Now;

            var parking = new DataAccess.Models.Parking
            {
                Location = request.Location,
                Name = request.Name,
                CreatedAt = currentDate,
                UpdatedAt = currentDate,
            };

            return await _parkingDataServices.CreateParking(parking);
        }
    }
}
