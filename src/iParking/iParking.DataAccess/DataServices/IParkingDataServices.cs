using iParking.DataAccess.Models;
using iParking.Domain.ParkingModels;

namespace iParking.DataAccess.DataServices
{
    public interface IParkingDataServices
    {
        Task<Parking> CreateParking(Parking newParking);
        Task<Parking> GetParking(int id);
        Task<List<Parking>> GetParkings();
        Task<List<NearbyParkingLot>> GetNearbyParkings();
        Task<Parking?> UpdateParking(Parking updateParking);
        Task<Parking?> DeleteParking(int id);
    }
}
