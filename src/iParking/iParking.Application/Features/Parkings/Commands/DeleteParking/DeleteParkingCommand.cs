using MediatR;

namespace iParking.Application.Features.Parkings.Commands.DeleteParking
{
    public class DeleteParkingCommand : IRequest<DataAccess.Models.Parking?>
    {
        public int Id { get; set; }
    }
}
