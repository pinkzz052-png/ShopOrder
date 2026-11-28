using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly IDbSession _dbSession;

    public OrderRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        const string sql = @"
                SELECT o.Id, o.CustomerId, o.OrderDate, o.Status, o.TotalAmount, o.Note,
                c.FullName AS CustomerName
				FROM Orders o
                INNER JOIN Customers c ON c.Id = o.CustomerId
                ORDER BY o.Id DESC";

        return await _dbSession.Connection.QueryAsync<Order>(sql, _dbSession.Transaction);
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT o.Id, o.CustomerId, o.OrderDate, o.Status, o.TotalAmount, o.Note,
                   c.FullName AS CustomerName
            FROM Orders o
            INNER JOIN Customers c ON c.Id = o.CustomerId
            WHERE o.Id = @Id;

            SELECT oi.Id, oi.OrderId, oi.ProductId, p.Name AS ProductName, oi.Quantity, oi.UnitPrice
            FROM OrderItems oi
            LEFT JOIN Products p ON p.Id = oi.ProductId
            WHERE oi.OrderId = @Id;";

        await using var multi =  await _dbSession.Connection.QueryMultipleAsync(
            sql, new { Id = id }, transaction: _dbSession.Transaction);
        
        var order = await multi.ReadSingleOrDefaultAsync<Order>();
        if (order is null) return null;

        order.Items = (await multi.ReadAsync<OrderItem>()).ToList();
        return order;
    }

    public async Task<int> CreateAsync(Order order)
    {
        const string sql = @"
                INSERT INTO Orders (CustomerId, TotalAmount, Note)
                OUTPUT INSERTED.Id
                VALUES (@CustomerId, @TotalAmount, @Note)";

        return await _dbSession.Connection.ExecuteScalarAsync<int>(sql, order, transaction: _dbSession.Transaction);
    }

    public async Task<Order?> UpdateStatusAsync(int id, string status)
    {
        const string sql = @"
                UPDATE Orders
                SET Status = @Status
                WHERE Id = @Id
                ";

        var rowAffected =  await _dbSession.Connection.ExecuteAsync(sql, new { Id = id, Status = status }, transaction: _dbSession.Transaction);
        
        if (rowAffected == 0) return null;
        
        return await GetByIdAsync(id);  
    }
}