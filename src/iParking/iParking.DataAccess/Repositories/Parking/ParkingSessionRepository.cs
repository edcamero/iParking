using System.Data;
using Dapper;
using iParking.Domain.Entities.Parking;
using iParking.Domain.Enums;
using iParking.Domain.Shared;
using iParking.DataAccess.Repositories.Base;

namespace iParking.DataAccess.Repositories.Parking
{
    /// <summary>
    /// Implementación del repositorio de sesiones de parqueadero.
    /// Maneja el flujo completo de entrada/salida de vehículos.
    /// </summary>
    public class ParkingSessionRepository : BaseRepository, IParkingSessionRepository
    {
        public ParkingSessionRepository(IDbConnection connection) : base(connection) { }

        public async Task<Result<ParkingSession>> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM ParkingSessions WHERE Id = @Id";

            try
            {
                var session = await ExecuteQueryFirstOrDefaultAsync<ParkingSession>(sql, new { Id = id });
                return session != null 
                    ? Result<ParkingSession>.Success(session) 
                    : Result<ParkingSession>.Failure("Sesión no encontrada");
            }
            catch (Exception ex)
            {
                return Result<ParkingSession>.Failure($"Error al obtener sesión: {ex.Message}");
            }
        }

        public async Task<Result<ParkingSession>> GetActiveByLicensePlateAsync(string licensePlate, int parkingLotId)
        {
            const string sql = @"SELECT * FROM ParkingSessions 
                                 WHERE LicensePlate = @LicensePlate 
                                 AND ParkingLotId = @ParkingLotId 
                                 AND Status = 1"; // 1 = Active

            try
            {
                var session = await ExecuteQueryFirstOrDefaultAsync<ParkingSession>(sql, new { LicensePlate = licensePlate, ParkingLotId = parkingLotId });
                return session != null 
                    ? Result<ParkingSession>.Success(session) 
                    : Result<ParkingSession>.Failure("No hay sesión activa para este vehículo");
            }
            catch (Exception ex)
            {
                return Result<ParkingSession>.Failure($"Error al buscar sesión activa: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ParkingSession>>> GetByParkingLotAsync(int parkingLotId, DateTime? startDate = null, DateTime? endDate = null)
        {
            string sql = @"SELECT * FROM ParkingSessions 
                          WHERE ParkingLotId = @ParkingLotId";

            if (startDate.HasValue)
                sql += " AND EntryTime >= @StartDate";
            
            if (endDate.HasValue)
                sql += " AND EntryTime <= @EndDate";

            sql += " ORDER BY EntryTime DESC";

            try
            {
                var sessions = await ExecuteQueryAsync<ParkingSession>(sql, new { ParkingLotId = parkingLotId, StartDate = startDate, EndDate = endDate });
                return Result<IEnumerable<ParkingSession>>.Success(sessions);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ParkingSession>>.Failure($"Error al listar sesiones: {ex.Message}");
            }
        }

        public async Task<Result<ParkingSession>> CreateEntryAsync(ParkingSession session)
        {
            const string sql = @"INSERT INTO ParkingSessions 
                                (ParkingLotId, LicensePlate, VehicleType, EntryTime, Status, ParkingSpotId, OperatorUserId, TicketCode, CreatedDate)
                                VALUES (@ParkingLotId, @LicensePlate, @VehicleType, @EntryTime, @Status, @ParkingSpotId, @OperatorUserId, @TicketCode, GETDATE());
                                SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                session.Status = ParkingSessionStatus.Active;
                session.CreatedDate = DateTime.Now;
                
                var id = await ExecuteScalarAsync<int>(sql, session);
                session.Id = id;
                return Result<ParkingSession>.Success(session);
            }
            catch (Exception ex)
            {
                return Result<ParkingSession>.Failure($"Error al registrar entrada: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateExitAsync(int sessionId, decimal totalAmount, int paymentMethod, int operatorUserId)
        {
            const string sql = @"UPDATE ParkingSessions SET
                                ExitTime = GETDATE(),
                                Status = 2, -- Completed
                                TotalAmount = @TotalAmount,
                                PaymentMethod = @PaymentMethod,
                                PaymentDate = GETDATE(),
                                IsPaid = 1,
                                OperatorUserId = @OperatorUserId
                                WHERE Id = @Id AND Status = 1";

            try
            {
                var affected = await ExecuteNonQueryAsync(sql, new 
                { 
                    Id = sessionId, 
                    TotalAmount = totalAmount, 
                    PaymentMethod = paymentMethod,
                    OperatorUserId = operatorUserId
                });
                
                return Result<bool>.Success(affected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al registrar salida y cobro: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetActiveSessionsCountAsync(int parkingLotId)
        {
            const string sql = @"SELECT COUNT(*) FROM ParkingSessions 
                                 WHERE ParkingLotId = @ParkingLotId AND Status = 1";

            try
            {
                var count = await ExecuteScalarAsync<int>(sql, new { ParkingLotId = parkingLotId });
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al contar sesiones activas: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> GetTotalRevenueAsync(int parkingLotId, DateTime startDate, DateTime endDate)
        {
            const string sql = @"SELECT ISNULL(SUM(TotalAmount), 0) FROM ParkingSessions 
                                 WHERE ParkingLotId = @ParkingLotId 
                                 AND IsPaid = 1 
                                 AND PaymentDate BETWEEN @StartDate AND @EndDate";

            try
            {
                var revenue = await ExecuteScalarAsync<decimal>(sql, new { ParkingLotId = parkingLotId, StartDate = startDate, EndDate = endDate });
                return Result<decimal>.Success(revenue);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Error al calcular ingresos: {ex.Message}");
            }
        }
    }
}
