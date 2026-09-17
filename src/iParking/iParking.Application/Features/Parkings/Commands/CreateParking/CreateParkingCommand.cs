using MediatR;

namespace iParking.Application.Features.Parkings.Commands.CreateParking
{
    public class CreateParkingCommand : IRequest<DataAccess.Models.Parking>
    {
        public string Location { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
