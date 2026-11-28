using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ShopOrder.DAL;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}