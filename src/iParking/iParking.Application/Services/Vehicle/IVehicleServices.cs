using iParking.Domain.Entities;
using iParking.Domain.Entities.Vehicle;

namespace iParking.Application.Services.Vehicle
{
    public interface IVehicleServices
    {
        Task<(List<EVehicle>, ActionResponse)> GetUserVehicles(int userId);
        Task<(EVehicle?, ActionResponseSession)> CreatedUserVehicle(VehicleUserInput vehicleInput);
        Task<ActionResponseSession> UpdatedUserVehicleDefault(int userId, int vehicleId);
        Task<ActionResponseSession> DeletedUserVehicle(int userId, int vehicleId);
    }
}
