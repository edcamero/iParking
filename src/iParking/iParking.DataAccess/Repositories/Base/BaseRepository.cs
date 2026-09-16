using System.Data;
using Microsoft.Data.SqlClient;

namespace iParking.DataAccess.Repositories.Base
{
    /// <summary>
    /// Repositorio base para operaciones comunes de base de datos.
    /// Implementa DRY centralizando lógica repetitiva de acceso a datos.
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        protected BaseRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        /// <summary>
        /// Ejecuta una consulta SQL parametrizada de forma segura.
        /// </summary>
        protected async Task<T?> ExecuteQueryAsync<T>(string query, Func<SqlDataReader, T> map, SqlParameter[]? parameters = null)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            using var command = new SqlCommand(query, connection);
            
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();
            
            if (reader.Read())
                return map(reader);

            return default;
        }

        /// <summary>
        /// Ejecuta una consulta SQL que retorna múltiples registros.
        /// </summary>
        protected async Task<List<T>> ExecuteQueryListAsync<T>(string query, Func<SqlDataReader, T> map, SqlParameter[]? parameters = null)
        {
            var results = new List<T>();
            
            using var connection = await _connectionFactory.GetConnectionAsync();
            using var command = new SqlCommand(query, connection);
            
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();
            
            while (reader.Read())
                results.Add(map(reader));

            return results;
        }

        /// <summary>
        /// Ejecuta un comando SQL (INSERT, UPDATE, DELETE) y retorna el número de filas afectadas.
        /// </summary>
        protected async Task<int> ExecuteCommandAsync(string query, SqlParameter[]? parameters = null)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            using var command = new SqlCommand(query, connection);
            
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Ejecuta un stored procedure y retorna el resultado escalar.
        /// </summary>
        protected async Task<T?> ExecuteStoredProcedureAsync<T>(string procedureName, SqlParameter[] parameters)
        {
            using var connection = await _connectionFactory.GetConnectionAsync();
            using var command = new SqlCommand(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            var result = await command.ExecuteScalarAsync();
            
            if (result == null || result == DBNull.Value)
                return default;

            return (T)Convert.ChangeType(result, typeof(T));
        }

        /// <summary>
        /// Crea un parámetro SQL de forma segura.
        /// </summary>
        protected SqlParameter CreateParameter(string name, object value, SqlDbType type, int size = 0)
        {
            var parameter = new SqlParameter(name, type)
            {
                Value = value ?? DBNull.Value
            };

            if (size > 0)
                parameter.Size = size;

            return parameter;
        }
    }
}
