using iParking.Domain.Entities.Usuario;
using iParking.Domain.Entities.Vehicle;
using Microsoft.Data.SqlClient;

namespace iParking.DataAccess.DataServices.VehicleServices
{
    public class VehicleDataServices : IVehicleDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public VehicleDataServices(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<EVehicle>> GetUserVehicles(int userId)
        {
            List<EVehicle> vehicles = new List<EVehicle>();

            using var connection = await _connectionFactory.GetConnectionAsync();

            string query = "SELECT * FROM TBL_PLACA WHERE ID_USUARIO = @IdUsuario";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdUsuario", userId);

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

        public async Task<int> CreatedUserVehicle(VehicleUserInput vehicleInput)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("sp_ingPlacas  @placa, @default, @useId, @fecharegistro, 1", connection);
            command.Parameters.AddWithValue("@placa", vehicleInput.Placa);
            command.Parameters.AddWithValue("@default", 1);
            command.Parameters.AddWithValue("@useId", vehicleInput.KeySesion);
            command.Parameters.AddWithValue("@fecharegistro", DateTime.Now.ToString());

            var result = await command.ExecuteScalarAsync();
            int id = Convert.ToInt32(result);

            return id;
        }

        public async Task<bool> UpdatedUserVehicleDefault(int userId, int vehicleId)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("UPDATE TBL_PLACA SET PLACA_DEFAULT=0 WHERE id_usuario = @userId;" +
                "UPDATE TBL_PLACA SET PLACA_DEFAULT =1 WHERE id_placa = @vehicleId", connection);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@vehicleId", vehicleId);

            var result = await command.ExecuteNonQueryAsync();

            return  Convert.ToInt32(result) > 0;
           
        }
    }
}

