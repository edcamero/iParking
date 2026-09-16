using iParking.DataAccess.Repositories.Base;
using iParking.Domain.Entities.Usuario;
using Microsoft.Data.SqlClient;
using System.Data;

namespace iParking.DataAccess.Repositories
{
    /// <summary>
    /// Implementación del repositorio de usuarios.
    /// Aplica principios SOLID y previene inyección SQL mediante parametrización.
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ISqlConnectionFactory connectionFactory) 
            : base(connectionFactory)
        {
        }

        public async Task<Usuario?> GetUserByRutAsync(string rut, string dv)
        {
            var query = "SELECT * FROM TBL_USUARIOS WHERE RUT = @rut AND DV = @dv";
            
            var parameters = new[]
            {
                CreateParameter("@rut", rut, SqlDbType.VarChar, 20),
                CreateParameter("@dv", dv, SqlDbType.VarChar, 1)
            };

            return await ExecuteQueryAsync(
                query,
                reader => MapToUsuario(reader, includePassword: true),
                parameters
            );
        }

        public async Task<Usuario?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM TBL_USUARIOS WHERE MAIL = @mail";
            
            var parameters = new[]
            {
                CreateParameter("@mail", email, SqlDbType.VarChar, 100)
            };

            return await ExecuteQueryAsync(
                query,
                reader => MapToUsuario(reader, includePassword: false),
                parameters
            );
        }

        public async Task<bool> UserExistsAsync(string rut, string dv)
        {
            var query = "SELECT COUNT(*) FROM TBL_USUARIOS WHERE RUT = @rut AND DV = @dv";
            
            var parameters = new[]
            {
                CreateParameter("@rut", rut, SqlDbType.VarChar, 20),
                CreateParameter("@dv", dv, SqlDbType.VarChar, 1)
            };

            var result = await ExecuteStoredProcedureAsync<int>(query, parameters);
            return result > 0;
        }

        public async Task<int> CreateUserAsync(UsuarioNuevo usuario)
        {
            var parameters = new[]
            {
                CreateParameter("@rut", usuario.Rut, SqlDbType.VarChar, 20),
                CreateParameter("@dv", usuario.DigVer, SqlDbType.VarChar, 1),
                CreateParameter("@nombres", usuario.Nombres, SqlDbType.VarChar, 100),
                CreateParameter("@apellidos", usuario.Apellidos, SqlDbType.VarChar, 100),
                CreateParameter("@telefono", usuario.Telefono, SqlDbType.VarChar, 20),
                CreateParameter("@clave_acceso", usuario.ClaveAcceso, SqlDbType.VarChar, 256),
                CreateParameter("@mail", usuario.Mail, SqlDbType.VarChar, 100),
                CreateParameter("@estado", 1, SqlDbType.Int)
            };

            return await ExecuteStoredProcedureAsync<int>("sp_ingUsuario", parameters);
        }

        public async Task<bool> LoginAsync(string email, string hashedPassword)
        {
            var query = "SELECT ID_USUARIO FROM TBL_USUARIOS WHERE MAIL = @mail AND CLAVE_ACCESO = @password";
            
            var parameters = new[]
            {
                CreateParameter("@mail", email, SqlDbType.VarChar, 100),
                CreateParameter("@password", hashedPassword, SqlDbType.VarChar, 256)
            };

            var result = await ExecuteStoredProcedureAsync<int>(query, parameters);
            return result > 0;
        }

        private static Usuario MapToUsuario(SqlDataReader reader, bool includePassword)
        {
            return new Usuario
            {
                IdUsuario = Convert.ToInt32(reader["ID_USUARIO"]),
                Rut = Convert.ToString(reader["RUT"]) ?? string.Empty,
                Dv = Convert.ToString(reader["DV"]) ?? string.Empty,
                Nombres = Convert.ToString(reader["NOMBRES"]) ?? string.Empty,
                Apellidos = Convert.ToString(reader["APELLIDOS"]) ?? string.Empty,
                Mail = Convert.ToString(reader["MAIL"]) ?? string.Empty,
                Telefono = Convert.ToString(reader["TELEFONO"]) ?? string.Empty,
                Estado = Convert.ToInt32(reader["ESTADO"]),
                ClaveAcceso = includePassword ? Convert.ToString(reader["CLAVE_ACCESO"]) ?? string.Empty : string.Empty
            };
        }
    }
}
