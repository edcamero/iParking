using MediatR;

namespace iParking.Application.Features.Parkings.Queries.GetParkings
{
    public class GetParkingsQuery : IRequest<List<DataAccess.Models.Parking>>
    {
    }
}
