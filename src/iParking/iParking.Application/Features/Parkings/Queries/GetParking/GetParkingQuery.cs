using MediatR;

namespace iParking.Application.Features.Parkings.Queries.GetParking
{
    public class GetParkingQuery : IRequest<DataAccess.Models.Parking>
    {
        public int Id { get; set; }
    }
}
