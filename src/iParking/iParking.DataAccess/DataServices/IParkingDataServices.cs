using iParking.DataAccess.Models;

namespace iParking.DataAccess.DataServices
{
    public interface IParkingDataServices
    {
        Task<Parking> CreateParking(Parking newParking);
        Task<Parking> GetParking(int id);
        Task<List<Parking>> GetParkings();
        Task<Parking?> UpdateParking(Parking updateParking);
        Task<Parking?> DeleteParking(int id);
    }
}
