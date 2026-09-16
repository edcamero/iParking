using System.Data;
using iParking.Domain.Entities.Parking;
using iParking.Domain.Shared;

namespace iParking.DataAccess.Repositories.Parking
{
    /// <summary>
    /// Interfaz para el repositorio de sesiones de parqueadero.
    /// Maneja entradas, salidas y cobros.
    /// </summary>
    public interface IParkingSessionRepository
    {
        Task<Result<ParkingSession>> GetByIdAsync(int id);
        Task<Result<ParkingSession>> GetActiveByLicensePlateAsync(string licensePlate, int parkingLotId);
        Task<Result<IEnumerable<ParkingSession>>> GetByParkingLotAsync(int parkingLotId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<ParkingSession>> CreateEntryAsync(ParkingSession session);
        Task<Result<bool>> UpdateExitAsync(int sessionId, decimal totalAmount, int paymentMethod, int operatorUserId);
        Task<Result<int>> GetActiveSessionsCountAsync(int parkingLotId);
        Task<Result<decimal>> GetTotalRevenueAsync(int parkingLotId, DateTime startDate, DateTime endDate);
    }
}
