using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;
using Microsoft.Data.SqlClient;

using System.Data;

namespace ShopOrder.DAL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public OrderRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            const string sql = @"
                SELECT o.Id AS OrderId, o.CustomerId, o.OrderDate, o.Status, o.TotalAmount, o.Note,
                c.FullName AS CustomerName
				FROM Orders o
                INNER JOIN Customers c ON c.Id = o.CustomerId
                ORDER BY o.Id DESC";
            
            var orders = new List<Order>();

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);

            await using var reader = await command.ExecuteReaderAsync();

            var idCol = reader.GetOrdinal("OrderId");
            var customerIdCol = reader.GetOrdinal("CustomerId");
            var orderDateCol = reader.GetOrdinal("OrderDate");
            var statusCol = reader.GetOrdinal("Status");
            var totalAmountCol = reader.GetOrdinal("TotalAmount");
            var noteCol = reader.GetOrdinal("Note");

            while (await reader.ReadAsync())
            {
                orders.Add(MapToOrder(reader));
            }

            return orders;
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT o.Id AS OrderId, o.CustomerId, o.OrderDate, o.Status, o.TotalAmount, o.Note,
                c.FullName AS CustomerName,
                oi.Id AS ItemId, oi.OrderId, oi.ProductId, oi.Quantity, oi.UnitPrice
                p.Name AS ProductName
                FROM Orders
                INNER JOIN Customers c ON c.Id = o.CustomerId,
                LEFT JOIN OrderItems oi ON oi.OrderId = o.Id,
                LEFT JOIN Products p ON p.Id = oi.ProductId
                WHERE Id =  @Id";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            Order? order = null;
            int itemIdCol = reader.GetOrdinal("ItemId");

            while (await reader.ReadAsync())
            {
                if (order is null)
                {
                    order = MapToOrder(reader);
                }
                if (!reader.IsDBNull(itemIdCol))
                {
                    order.Items.Add(new OrderItem
                    {
                        Id = reader.GetInt32(itemIdCol),
                        OrderId = order.Id,
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"))
                    });
                }
            }
            return order;
        }

        public async Task<Order?> CreateAsync(Order order)
        {
            const string insertOrderSql = @"
                INSERT INTO Orders (CustomerId, TotalAmount, Note)
                OUPUT INSERTED.Id
                VALUES (@CustomerId, @TotalAmount, @Note)";
            const string insertItemSql = @"
                INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice)
                VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            int newOrderId;

            try
            {
                await using (var orderCommand = new SqlCommand(insertOrderSql, connection, transaction))
                {
                    orderCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = order.CustomerId;
                    var price = orderCommand.Parameters.Add("@TotalAmount", SqlDbType.Decimal);
                    price.Precision = 18;
                    price.Scale = 2;
                    price.Value = order.TotalAmount;

                    orderCommand.Parameters.Add("@Note", SqlDbType.NVarChar, 500).Value = (object?)order.Note ?? DBNull.Value ;
                    newOrderId = Convert.ToInt32(await orderCommand.ExecuteScalarAsync());
                }
                foreach (var item in order.Items)
                {

                }
            }
            catch
            {

            }
            return null;
        }
        public async Task<Order?> UpdateStatusAsync(int id, string status)
        {
            const string sql = @"
                UPDATE Orders
                SET Status = @Status
                WHERE Id = @Id
                ";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = status;

            var rowAffected = await command.ExecuteNonQueryAsync();
            if (rowAffected == 0)
            {
                return null;
            }

            return await GetByIdAsync(id);
        }

        public static Order MapToOrder(SqlDataReader reader) => new Order
        {
            Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
            CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
            Note = reader.IsDBNull(reader.GetOrdinal("Note")) ? null : reader.GetString(reader.GetOrdinal("Note"))
        };
    }
}
