using System.Data;
using Dapper;
using iParking.Domain.Entities.Parking;
using iParking.Domain.Shared;
using iParking.DataAccess.Repositories.Base;

namespace iParking.DataAccess.Repositories.Parking
{
    /// <summary>
    /// Implementación del repositorio de parqueaderos.
    /// </summary>
    public class ParkingLotRepository : BaseRepository, IParkingLotRepository
    {
        public ParkingLotRepository(IDbConnection connection) : base(connection) { }

        public async Task<Result<ParkingLot>> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM ParkingLots 
                                 WHERE Id = @Id AND IsActive = 1";

            try
            {
                var parkingLot = await ExecuteQueryFirstOrDefaultAsync<ParkingLot>(sql, new { Id = id });
                return parkingLot != null 
                    ? Result<ParkingLot>.Success(parkingLot) 
                    : Result<ParkingLot>.Failure("Parqueadero no encontrado");
            }
            catch (Exception ex)
            {
                return Result<ParkingLot>.Failure($"Error al obtener parqueadero: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ParkingLot>>> GetByCompanyAsync(int companyId)
        {
            const string sql = @"SELECT * FROM ParkingLots 
                                 WHERE CompanyId = @CompanyId AND IsActive = 1
                                 ORDER BY Name";

            try
            {
                var parkingLots = await ExecuteQueryAsync<ParkingLot>(sql, new { CompanyId = companyId });
                return Result<IEnumerable<ParkingLot>>.Success(parkingLots);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ParkingLot>>.Failure($"Error al listar parqueaderos: {ex.Message}");
            }
        }

        public async Task<Result<ParkingLot>> CreateAsync(ParkingLot parkingLot)
        {
            const string sql = @"INSERT INTO ParkingLots 
                                (CompanyId, Name, Address, City, Latitude, Longitude, OpeningTime, ClosingTime, 
                                 Is24Hours, TotalCapacity, CreatedDate, IsActive)
                                 VALUES (@CompanyId, @Name, @Address, @City, @Latitude, @Longitude, @OpeningTime, @ClosingTime,
                                         @Is24Hours, @TotalCapacity, GETDATE(), 1);
                                 SELECT CAST(SCOPE_IDENTITY() as int);";

            try
            {
                var id = await ExecuteScalarAsync<int>(sql, parkingLot);
                parkingLot.Id = id;
                return Result<ParkingLot>.Success(parkingLot);
            }
            catch (Exception ex)
            {
                return Result<ParkingLot>.Failure($"Error al crear parqueadero: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateAsync(ParkingLot parkingLot)
        {
            const string sql = @"UPDATE ParkingLots SET
                                Name = @Name,
                                Address = @Address,
                                City = @City,
                                Latitude = @Latitude,
                                Longitude = @Longitude,
                                OpeningTime = @OpeningTime,
                                ClosingTime = @ClosingTime,
                                Is24Hours = @Is24Hours,
                                TotalCapacity = @TotalCapacity,
                                ModifiedDate = GETDATE()
                                WHERE Id = @Id AND CompanyId = @CompanyId";

            try
            {
                var affected = await ExecuteNonQueryAsync(sql, parkingLot);
                return Result<bool>.Success(affected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al actualizar parqueadero: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            const string sql = "UPDATE ParkingLots SET IsActive = 0, ModifiedDate = GETDATE() WHERE Id = @Id";

            try
            {
                var affected = await ExecuteNonQueryAsync(sql, new { Id = id });
                return Result<bool>.Success(affected > 0);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar parqueadero: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetAvailableSpotsCountAsync(int parkingLotId)
        {
            const string sql = @"SELECT COUNT(*) FROM ParkingSpots 
                                 WHERE ParkingLotId = @ParkingLotId AND Status = 1"; // 1 = Available

            try
            {
                var count = await ExecuteScalarAsync<int>(sql, new { ParkingLotId = parkingLotId });
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al contar puestos disponibles: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetOccupiedSpotsCountAsync(int parkingLotId)
        {
            const string sql = @"SELECT COUNT(*) FROM ParkingSpots 
                                 WHERE ParkingLotId = @ParkingLotId AND Status = 2"; // 2 = Occupied

            try
            {
                var count = await ExecuteScalarAsync<int>(sql, new { ParkingLotId = parkingLotId });
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al contar puestos ocupados: {ex.Message}");
            }
        }
    }
}
