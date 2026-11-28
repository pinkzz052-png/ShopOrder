
using Microsoft.Data.SqlClient;
using ShopOrder.DAL;
using System.Data.Common;

namespace ShopOrder.Api;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("chuỗi kết nối không được để trống.", nameof(connectionString));
        _connectionString = connectionString;
    }

    public DbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}