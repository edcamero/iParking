using iParking.Domain.Entities.Vehicle;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iParking.DataAccess.DataServices.VehicleDataServices
{
    public class VehicleData : IVehicleDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public VehicleData(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<EVehicle>> GetUserVehicles(int userId)
        {
            List<EVehicle> vehicles = new List<EVehicle>();

            using var connection = await _connectionFactory.GetConnectionAsync();

            string query = string.Format("SELECT * FROM TBL_PLACA WHERE ID_USUARIO = {0} and ESTADO=1", userId);

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EVehicle placa = new EVehicle();
                        placa.IdPlaca = Convert.ToDecimal(reader["ID_PLACA"]);
                        placa.Placa = reader["PLACA"].ToString() ?? string.Empty;
                        placa.PlacaDefault = reader["PLACA_DEFAULT"] == DBNull.Value ? null : (decimal?)reader["PLACA_DEFAULT"];
                        placa.IdUsuario = reader["ID_USUARIO"] == DBNull.Value ? null : (decimal?)reader["ID_USUARIO"];
                        placa.FechaHoraCreado = reader["FECHA_HORA_CREADO"].ToString() ?? string.Empty;
                        placa.Estado = reader["ESTADO"] == DBNull.Value ? null : (decimal?)reader["ESTADO"];

                        vehicles.Add(placa);
                    }
                }
            }

            return vehicles;
        }

        public async Task<EVehicle> GetUserVehicle(int userId, int vehicleId)
        {
            EVehicle placa = new EVehicle();

            using var connection = await _connectionFactory.GetConnectionAsync();

            string query = string.Format("SELECT * FROM TBL_PLACA WHERE ID_USUARIO = {0} and id_placa = {1}", userId, vehicleId);

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    placa.IdPlaca = Convert.ToDecimal(reader["ID_PLACA"]);
                    placa.Placa = reader["PLACA"].ToString() ?? string.Empty;
                    placa.PlacaDefault = reader["PLACA_DEFAULT"] == DBNull.Value ? null : (decimal?)reader["PLACA_DEFAULT"];
                    placa.IdUsuario = reader["ID_USUARIO"] == DBNull.Value ? null : (decimal?)reader["ID_USUARIO"];
                    placa.FechaHoraCreado = reader["FECHA_HORA_CREADO"].ToString() ?? string.Empty;
                    placa.Estado = reader["ESTADO"] == DBNull.Value ? null : (decimal?)reader["ESTADO"];
                }
            }

            return placa;
        }

        public async Task<int> CreatedUserVehicle(VehicleUserInput vehicleInput, int isDefault)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("sp_ingPlacas", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@placa", vehicleInput.Placa);
            command.Parameters.AddWithValue("@placa_default", isDefault);
            command.Parameters.AddWithValue("@id_usuario", vehicleInput.KeySession);
            command.Parameters.AddWithValue("@fecha_hora_creado", DateTime.Now.ToString());
            command.Parameters.AddWithValue("@estado", 1);

            var result = await command.ExecuteScalarAsync();
            int id = Convert.ToInt32(result);

            return id;
        }

        public async Task<bool> UpdatedUserVehicleDefault(int userId, int vehicleId)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("UPDATE TBL_PLACA SET PLACA_DEFAULT=0 WHERE id_usuario = {0}; UPDATE TBL_PLACA SET PLACA_DEFAULT =1 WHERE id_placa = {1}", userId, vehicleId);
            using var command = new SqlCommand(query, connection);

            var result = await command.ExecuteNonQueryAsync();

            return Convert.ToInt32(result) > 0;
        }

        public async Task<bool> DeleteUserVehicle(int userId, int vehicleId)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            var query = string.Format("UPDATE TBL_PLACA SET ESTADO = 0 WHERE id_usuario = {0} and id_placa = {1}", userId, vehicleId);
            using var command = new SqlCommand(query, connection);

            var result = await command.ExecuteNonQueryAsync();

            return Convert.ToInt32(result) > 0;
        }
    }
}

