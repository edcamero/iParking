using iParking.Domain.Entities.VehicleOwner;
using iParking.Domain.Entities;

namespace iParking.Application.Services.User
{
    public interface IUserServices
    {
        Task<ActionResponseSession> CreatedUser(NewVehicleOwner nuevoUsuario);
    }
}
