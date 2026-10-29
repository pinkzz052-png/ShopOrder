using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOrder.DAL
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
