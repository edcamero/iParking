﻿using iParking.Domain.Entities.Usuario;
using Microsoft.Data.SqlClient;

namespace iParking.DataAccess.DataServices
{
    public class UserDataServices: IUserDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserDataServices(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Usuario?> GetUserAsync(string rut, string dv)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = "SELECT * FROM TBL_USUARIOS WHERE RUT = @rut AND DV = @dv";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@rut", rut);
            command.Parameters.AddWithValue("@dv", dv);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                        Rut = rut,
                        Dv = dv,
                        Nombres = Convert.ToString(reader["NOMBRES"]) ?? string.Empty,
                        Apellidos = Convert.ToString(reader["APELLIDOS"]) ?? string.Empty,
                        Mail = Convert.ToString(reader["MAIL"]) ?? string.Empty,
                        Telefono = Convert.ToString(reader["TELEFONO"]) ?? string.Empty,
                        Estado = Convert.ToInt32(reader["ESTADO"]),
                    };

                    return usuario;
                }
            }

            return null;
        }

        public async Task<bool> CheckIfUserExists(string rut, string dv)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("Select COUNT(*) from TBL_USUARIOS where rut=@rut  AND dv= @dv", connection);
            command.Parameters.AddWithValue("@rut", rut);
            command.Parameters.AddWithValue("@dv", dv);

            var result = await command.ExecuteScalarAsync();
            int count = Convert.ToInt32(result);

            return count > 0;
        }

        public async Task<int> CreatedUser(UsuarioNuevo nuevoUsuario)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("sp_ingUsuario @rut, @dv, @nombres, @apellidos, @telefono, @clave_acceso, @mail, 1", connection);
            command.Parameters.AddWithValue("@rut", nuevoUsuario.Rut);
            command.Parameters.AddWithValue("@dv", nuevoUsuario.DigVer);
            command.Parameters.AddWithValue("@nombres", nuevoUsuario.Nombres);
            command.Parameters.AddWithValue("@apellidos", nuevoUsuario.Apellidos);
            command.Parameters.AddWithValue("@telefono", nuevoUsuario.Telefono);
            command.Parameters.AddWithValue("@clave_acceso", nuevoUsuario.ClaveAcceso);
            command.Parameters.AddWithValue("@mail", nuevoUsuario.Mail);

            var result = await command.ExecuteScalarAsync();
            int id = Convert.ToInt32(result);

            return id;
        }
    }
}
