using Microsoft.Data.SqlClient;

namespace iParking.DataAccess
{
    public interface ISqlConnectionFactory
    {
        Task<SqlConnection> GetConnectionAsync();
    }
}
