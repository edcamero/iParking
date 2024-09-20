using iParking.Domain.Entities.Vehicle;

namespace iParking.DataAccess.DataServices.VehicleDataServices
{
    public interface IVehicleDataServices
    {
        Task<List<EVehicle>> GetUserVehicles(int userId);
        Task<int> CreatedUserVehicle(VehicleUserInput vehicleInput, int isDefault);
        Task<bool> UpdatedUserVehicleDefault(int userId, int vehicleId);
        Task<bool> DeleteUserVehicle(int userId, int vehicleId);
        Task<EVehicle> GetUserVehicle(int userId, int vehicleId);
    }
}
