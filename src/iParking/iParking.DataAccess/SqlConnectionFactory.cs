using Microsoft.Data.SqlClient;
using System.Data;

namespace iParking.DataAccess
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection? _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> GetConnectionAsync()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
                await _connection.OpenAsync();
            }
            else if (_connection.State != ConnectionState.Open)
            {
                _connection.ConnectionString = _connectionString;
                await _connection.OpenAsync();
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }

}
