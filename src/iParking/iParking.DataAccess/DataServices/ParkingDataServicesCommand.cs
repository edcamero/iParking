using iParking.DataAccess.Models;
using Microsoft.Data.SqlClient;

namespace iParking.DataAccess.DataServices
{
    public class ParkingDataServicesCommand : IParkingDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public ParkingDataServicesCommand(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Parking> CreateParking(Parking newParking)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("INSERT INTO Parking (name, location, createdAt) VALUES (@name, @location, @createdAt); SELECT SCOPE_IDENTITY();", connection);
            command.Parameters.AddWithValue("@name", newParking.Name);
            command.Parameters.AddWithValue("@location", newParking.Location);
            command.Parameters.AddWithValue("@createdAt", newParking.CreatedAt);

            newParking.Id = Convert.ToInt32(command.ExecuteScalar());
            
            return newParking;
        }

        public async Task<Parking?> GetParking(int id)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            await connection.OpenAsync();

            using var command = new SqlCommand("SELECT * FROM Parking WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var parking = new Parking
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Location = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("createdAt")),
                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updatedAt")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("updatedAt"))
                };

                return parking;
            }

            return null;
        }

        public async Task<List<Parking>> GetParkings()
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("SELECT * FROM Parking;", connection);

            var parkings = new List<Parking>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var parking = new Parking
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Location = reader.IsDBNull(reader.GetOrdinal("location")) ? null : reader.GetString(reader.GetOrdinal("location")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("createdAt")),
                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updatedAt")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("updatedAt"))
                };

                parkings.Add(parking);
            }

            return parkings;
        }

        public async Task<Parking?> UpdateParking(Parking updateParking)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("UPDATE Parking SET name = @name, location = @location, updatedAt = @updatedAt WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@name", updateParking.Name);
            command.Parameters.AddWithValue("@location", updateParking.Location);
            command.Parameters.AddWithValue("@updatedAt", updateParking.UpdatedAt);
            command.Parameters.AddWithValue("@id", updateParking.Id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                return updateParking;
            }

            return null;
        }

        public async Task<Parking?> DeleteParking(int id)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("DELETE FROM Parking WHERE id = @id;", connection);
            command.Parameters.AddWithValue("@id", id);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                var deletedParking = new Parking { Id = id };
                return deletedParking;
            }

            return null;
        }
    }
}
