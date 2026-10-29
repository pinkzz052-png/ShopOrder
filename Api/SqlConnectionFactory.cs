
using Microsoft.Data.SqlClient;
using ShopOrder.DAL;

namespace ShopOrder.Api
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("chuỗi kết nối không được để trống.", nameof(connectionString));
            }
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
