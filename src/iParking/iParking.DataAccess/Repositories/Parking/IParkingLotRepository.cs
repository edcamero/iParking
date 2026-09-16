using System.Data;
using iParking.Domain.Entities.Parking;
using iParking.Domain.Shared;

namespace iParking.DataAccess.Repositories.Parking
{
    /// <summary>
    /// Interfaz para el repositorio de parqueaderos.
    /// </summary>
    public interface IParkingLotRepository
    {
        Task<Result<ParkingLot>> GetByIdAsync(int id);
        Task<Result<IEnumerable<ParkingLot>>> GetByCompanyAsync(int companyId);
        Task<Result<ParkingLot>> CreateAsync(ParkingLot parkingLot);
        Task<Result<bool>> UpdateAsync(ParkingLot parkingLot);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<int>> GetAvailableSpotsCountAsync(int parkingLotId);
        Task<Result<int>> GetOccupiedSpotsCountAsync(int parkingLotId);
    }
}
