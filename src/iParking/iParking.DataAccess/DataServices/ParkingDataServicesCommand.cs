using iParking.DataAccess.Models;
using iParking.Domain.ParkingModels;
using Microsoft.Data.SqlClient;
using System;

namespace iParking.DataAccess.DataServices
{
    public class ParkingDataServicesCommand : IParkingDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private string FormateDateString = "yyyy-MM-dd HH:mm:ss";

        public ParkingDataServicesCommand(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Parking> CreateParking(Parking newParking)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            var query = string.Format("INSERT INTO Parking (name, location, createdAt) VALUES ('{0}', '{1}', '{2}'); SELECT SCOPE_IDENTITY();", newParking.Name, newParking.Location, newParking.CreatedAt.ToString(FormateDateString));
            using var command = new SqlCommand(query, connection);

            newParking.Id = Convert.ToInt32(command.ExecuteScalar());
            
            return newParking;
        }

        public async Task<Parking?> GetParking(int id)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            var query = string.Format("SELECT * FROM Parking WHERE id = {0};", id);

            using var command = new SqlCommand(query, connection);

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
        
        public async Task<List<NearbyParkingLot>> GetNearbyParkings()
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("SELECT * FROM TBL_LUGARES WHERE ESTADO = 1 ", connection);

            var parkings = new List<NearbyParkingLot>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var parking = new NearbyParkingLot
                {
                    Id=Convert.ToInt32(reader["ID_LUGAR"]),
                    Name = reader.GetString(reader.GetOrdinal("NOMBRE")),
                    Latitude = reader.GetDouble(reader.GetOrdinal("LATITUD")),
                    Longitude = reader.GetDouble(reader.GetOrdinal("LONGITUD")),
                    Tariff = reader.GetString(reader.GetOrdinal("TARIFA")),
                    Schedule = reader.GetString(reader.GetOrdinal("HORARIO")),
                    AvailablePlaces = Convert.ToInt32(reader["LUGARES_DISPONIBLES"]),
                    State = reader.GetBoolean(reader.GetOrdinal("ESTADO")),
                };

                parkings.Add(parking);
            }

            return parkings;
        }

        public async Task<Parking?> UpdateParking(Parking updateParking)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("UPDATE Parking SET name = '{0}', location = '{1}', updatedAt = '{2}' WHERE id = {3};", updateParking.Name, updateParking.Location, updateParking.UpdatedAt?.ToString(FormateDateString), updateParking.Id);

            using var command = new SqlCommand(query, connection);

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

            var query = string.Format("DELETE FROM Parking WHERE id = {0};",id);

            using var command = new SqlCommand(query, connection);

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
