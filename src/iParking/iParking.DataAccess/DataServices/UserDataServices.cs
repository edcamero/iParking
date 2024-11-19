using iParking.Domain.Entities.Auth;
using iParking.Domain.Entities.Usuario;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iParking.DataAccess.DataServices
{
    public class UserDataServices : IUserDataServices
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserDataServices(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Usuario?> GetUserAsync(string rut, string dv)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("SELECT * FROM TBL_USUARIOS WHERE RUT = '{0}' AND DV = '{1}'", rut, dv);
            using var command = new SqlCommand(query, connection);

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
                        ClaveAcceso = Convert.ToString(reader["CLAVE_ACCESO"]) ?? string.Empty
                    };

                    return usuario;
                }
            }

            return null;
        }

        public async Task<Usuario?> GetUserAsync(string mail)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("SELECT * FROM TBL_USUARIOS WHERE MAIL = '{0}'", mail);
            using var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                        Rut = Convert.ToString(reader["RUT"]) ?? string.Empty,
                        Dv = Convert.ToString(reader["DV"]) ?? string.Empty,   
                        Nombres = Convert.ToString(reader["NOMBRES"]) ?? string.Empty,
                        Apellidos = Convert.ToString(reader["APELLIDOS"]) ?? string.Empty,
                        Mail = Convert.ToString(reader["MAIL"]) ?? string.Empty,
                        Telefono = Convert.ToString(reader["TELEFONO"]) ?? string.Empty,
                        Estado = Convert.ToInt32(reader["ESTADO"]),
                        ClaveAcceso = string.Empty
                    };

                    return usuario;
                }
            }

            return null;
        }

        public async Task<bool> CheckIfUserExists(string rut, string dv)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            var query = string.Format("Select COUNT(*) from TBL_USUARIOS where rut='{0}'  AND dv= '{1}'", rut, dv);
            using var command = new SqlCommand(query, connection);

            var result = await command.ExecuteScalarAsync();
            int count = Convert.ToInt32(result);

            return count > 0;
        }

        public async Task<int> CreatedUser(UsuarioNuevo nuevoUsuario)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();

            using var command = new SqlCommand("sp_ingUsuario", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@rut", nuevoUsuario.Rut);
            command.Parameters.AddWithValue("@dv", nuevoUsuario.DigVer);
            command.Parameters.AddWithValue("@nombres", nuevoUsuario.Nombres);
            command.Parameters.AddWithValue("@apellidos", nuevoUsuario.Apellidos);
            command.Parameters.AddWithValue("@telefono", nuevoUsuario.Telefono);
            command.Parameters.AddWithValue("@clave_acceso", nuevoUsuario.ClaveAcceso);
            command.Parameters.AddWithValue("@mail", nuevoUsuario.Mail);
            command.Parameters.AddWithValue("@estado", 1);

            var result = await command.ExecuteScalarAsync();
            int id = Convert.ToInt32(result);

            return id;
        }

        public async Task<bool> Login(LoginInput login)
        {
            try
            {
                using var connection = await _connectionFactory.GetConnectionAsync();
                using var command = new SqlCommand(
                    "SELECT ID_USUARIO FROM TBL_USUARIOS WHERE MAIL = @mail AND CLAVE_ACCESO = @password",
                    connection);

                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar) { Value = login.Mail });
                command.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = login.ClaveAcceso });

                var result = await command.ExecuteScalarAsync();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    return id > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Manejo del error
                throw new Exception("Error al intentar iniciar sesión", ex);
            }
        }

    }
}
