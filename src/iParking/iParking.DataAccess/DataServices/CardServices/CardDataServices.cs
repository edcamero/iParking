using iParking.Domain.Entities;
using iParking.Domain.Entities.Payment;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iParking.DataAccess.DataServices.CardServices
{
    public class CardDataServices: ICardDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public CardDataServices(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<CreditCard?> GetCardAsync(string numero_tarjeta, string cvv, string tipo)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("Select * from TBL_TARJETA where numero_tarjeta= {0} AND cvv= {1} AND tipo= {2}", numero_tarjeta.Trim(), cvv.Trim(), tipo.Trim());
            using var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    CreditCard creditCard = new CreditCard
                    {
                        IdTarjeta= Convert.ToInt32(reader["ID_TARJETA"]),
                        NumeroTarjeta = Convert.ToInt32(reader["NUMERO_TARJETA"]),
                        MesVcto = Convert.ToInt32(reader["MES_VCTO"]),
                        AnoVcto = Convert.ToInt32(reader["ANO_VCTO"]),
                        Cvv = Convert.ToInt32(reader["CVV"]),
                        Tipo = Convert.ToInt32(reader["TIPO"]),
                        IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                        FechaHoraCreado = Convert.ToString(reader["FECHA_HORA_CREADO"])?? string.Empty,
                        Estado = Convert.ToInt32(reader["ESTADO"]),
                    };

                    return creditCard;
                }
            }

            return null;
        }

        public async Task<CreditCard?> GetCardAsync(int userId, int creditCardId)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = "Select * from TBL_TARJETA where ID_TARJETA= @creeditCardId AND ID_USUARIO = @userId";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@creeditCardId", creditCardId);
            command.Parameters.AddWithValue("@userId", userId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    CreditCard creditCard = new CreditCard
                    {
                        IdTarjeta = Convert.ToInt32(reader["ID_TARJETA"]),
                        NumeroTarjeta = Convert.ToInt32(reader["NUMERO_TARJETA"]),
                        MesVcto = Convert.ToInt32(reader["MES_VCTO"]),
                        AnoVcto = Convert.ToInt32(reader["ANO_VCTO"]),
                        Cvv = Convert.ToInt32(reader["CVV"]),
                        Tipo = Convert.ToInt32(reader["TIPO"]),
                        IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                        FechaHoraCreado = Convert.ToString(reader["FECHA_HORA_CREADO"]) ?? string.Empty,
                        Estado = Convert.ToInt32(reader["ESTADO"]),
                    };

                    return creditCard;
                }
            }

            return null;
        }

        public async Task<int> CreatedCreditCard(CreditCardInput creditCard)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            var query = "EXEC sp_ingTarjetas";
            using var command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@numeroTarjeta", creditCard.NumeroTarjeta);
            command.Parameters.AddWithValue("@mesVencimiento", creditCard.MesVcto);
            command.Parameters.AddWithValue("@anioVencimiento", creditCard.AnoVcto);
            command.Parameters.AddWithValue("@cvv", creditCard.CvTarjeta);
            command.Parameters.AddWithValue("@tipo", creditCard.Tipo);
            command.Parameters.AddWithValue("@default", 0);
            command.Parameters.AddWithValue("@idUsuario", creditCard.IdUsuario);
            command.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
            command.Parameters.AddWithValue("@estadoHabilitado", 1);

            var result = await command.ExecuteScalarAsync();
            int id = Convert.ToInt32(result);

            return id;
        }

        public async Task<bool> DeletedCreditCard(int userId, int creditCardId)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("UPDATE TBL_TARJETA SET DEFAULT_TARJETA = 0 WHERE ID_USUARIO ={0} and  id_tarjeta = {1}", userId, creditCardId);
            using var command = new SqlCommand(query, connection);

            var result = await command.ExecuteNonQueryAsync();

            return Convert.ToInt32(result) > 0;
        }

        public async Task<bool> UpdatedCreditCardDefault(int userId, int creditCardId)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            var query = string.Format("UPDATE TBL_TARJETA SET DEFAULT_TARJETA = 0 WHERE id_usuario = {0}; UPDATE TBL_TARJETA SET DEFAULT_TARJETA=1 WHERE id_tarjeta = {1}", userId, creditCardId);
            using var command = new SqlCommand(query, connection);

            var result = await command.ExecuteNonQueryAsync();

            return Convert.ToInt32(result) > 0;
        }
    }
}
