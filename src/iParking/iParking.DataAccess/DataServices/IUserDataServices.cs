using iParking.Domain.Entities.VehicleOwner;

namespace iParking.DataAccess.DataServices
{
    public interface IUserDataServices
    {
        Task<bool> CheckIfUserExists(string rut, string dv);
        Task<VehicleOwner?> GetUserAsync(string? rut, string? dv);
        Task<int> CreatedUser(NewVehicleOwner nuevoUsuario);
    }
}
