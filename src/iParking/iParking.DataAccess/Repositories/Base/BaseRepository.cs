using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace iParking.DataAccess.Repositories.Base
{
    /// <summary>
    /// Repositorio base para operaciones comunes de base de datos.
    /// Soporta tanto consultas tradicionales ADO.NET con ISqlConnectionFactory
    /// como operaciones ágiles de alto rendimiento con Dapper e IDbConnection.
    /// Centraliza la ejecución segura, parametrización y mapeo de enums.
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly ISqlConnectionFactory? _connectionFactory;
        private readonly IDbConnection? _dbConnection;

        /// <summary>
        /// Constructor para repositorios basados en fábrica de conexiones SQL.
        /// </summary>
        protected BaseRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        /// <summary>
        /// Constructor para repositorios basados en IDbConnection (Dapper).
        /// </summary>
        protected BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        /// <summary>
        /// Obtiene una conexión abierta a la base de datos de manera segura.
        /// </summary>
        protected async Task<IDbConnection> GetOpenConnectionAsync()
        {
            if (_dbConnection != null)
            {
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();
                return _dbConnection;
            }

            if (_connectionFactory != null)
            {
                return await _connectionFactory.GetConnectionAsync();
            }

            throw new InvalidOperationException("No se ha configurado ninguna conexión ni fábrica de conexiones.");
        }

        #region Métodos Dapper

        /// <summary>
        /// Ejecuta una consulta SQL con Dapper y retorna el primer elemento encontrado o null.
        /// </summary>
        protected async Task<T?> ExecuteQueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
        {
            var connection = await GetOpenConnectionAsync();
            var shouldDispose = _connectionFactory != null;

            try
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            }
            finally
            {
                if (shouldDispose)
                    connection.Dispose();
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL con Dapper y retorna una colección de resultados.
        /// </summary>
        protected async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sql, object? parameters = null)
        {
            var connection = await GetOpenConnectionAsync();
            var shouldDispose = _connectionFactory != null;

            try
            {
                return await connection.QueryAsync<T>(sql, parameters);
            }
            finally
            {
                if (shouldDispose)
                    connection.Dispose();
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL con Dapper y retorna un valor escalar (ej: SCOPE_IDENTITY, COUNT).
        /// </summary>
        protected async Task<T?> ExecuteScalarAsync<T>(string sql, object? parameters = null)
        {
            var connection = await GetOpenConnectionAsync();
            var shouldDispose = _connectionFactory != null;

            try
            {
                return await connection.ExecuteScalarAsync<T>(sql, parameters);
            }
            finally
            {
                if (shouldDispose)
                    connection.Dispose();
            }
        }

        /// <summary>
        /// Ejecuta un comando SQL con Dapper (INSERT, UPDATE, DELETE) y retorna el número de filas afectadas.
        /// </summary>
        protected async Task<int> ExecuteNonQueryAsync(string sql, object? parameters = null)
        {
            var connection = await GetOpenConnectionAsync();
            var shouldDispose = _connectionFactory != null;

            try
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
            finally
            {
                if (shouldDispose)
                    connection.Dispose();
            }
        }

        #endregion

        #region Mapeo Seguro de Enums

        /// <summary>
        /// Mapea de forma segura un valor de BD a un enum C#.
        /// </summary>
        protected TEnum MapEnum<TEnum>(object? value) where TEnum : struct, Enum
        {
            return EnumHelper.MapEnum<TEnum>(value);
        }

        /// <summary>
        /// Mapea de forma segura un valor nullable de BD a un enum C# nullable.
        /// </summary>
        protected TEnum? MapEnumNullable<TEnum>(object? value) where TEnum : struct, Enum
        {
            return EnumHelper.MapEnumNullable<TEnum>(value);
        }

        #endregion

        #region Métodos Legados ADO.NET (ISqlConnectionFactory)

        /// <summary>
        /// Ejecuta una consulta SQL parametrizada de forma segura usando SqlDataReader.
        /// </summary>
        protected async Task<T?> ExecuteQueryAsync<T>(string query, Func<SqlDataReader, T> map, SqlParameter[]? parameters = null)
        {
            if (_connectionFactory == null)
                throw new InvalidOperationException("Esta operación requiere ISqlConnectionFactory.");

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
        /// Ejecuta una consulta SQL que retorna múltiples registros usando SqlDataReader.
        /// </summary>
        protected async Task<List<T>> ExecuteQueryListAsync<T>(string query, Func<SqlDataReader, T> map, SqlParameter[]? parameters = null)
        {
            if (_connectionFactory == null)
                throw new InvalidOperationException("Esta operación requiere ISqlConnectionFactory.");

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
            if (_connectionFactory == null)
                throw new InvalidOperationException("Esta operación requiere ISqlConnectionFactory.");

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
            if (_connectionFactory == null)
                throw new InvalidOperationException("Esta operación requiere ISqlConnectionFactory.");

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

        #endregion
    }
}
