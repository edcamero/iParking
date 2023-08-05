using iParking.Domain.Entities.Vehicle;

namespace iParking.DataAccess.DataServices.VehicleServices
{
    public interface IVehicleDataServices
    {
        Task<List<EVehicle>> GetUserVehicles(int userId);
        Task<int> CreatedUserVehicle(VehicleUserInput vehicleInput);
        Task<bool> UpdatedUserVehicleDefault(int userId, int vehicleId);
    }
}
