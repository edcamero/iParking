using MediatR;

namespace iParking.Application.Features.Parkings.Commands.UpdateParking
{
    public class UpdateParkingCommand : IRequest<DataAccess.Models.Parking?>
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
