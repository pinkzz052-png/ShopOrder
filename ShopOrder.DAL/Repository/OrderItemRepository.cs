using Dapper;
using Microsoft.Data.SqlClient;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Repository;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly IDbSession _dbSession;

    public OrderItemRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }
    public async Task<bool> CreateOrderItemAsync(List<OrderItem> orderItems)
    {
        const string sql = @"
            INSERT INTO OrderItems(OrderId, ProductId, Quantity, UnitPrice)
            VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)
            ";
        
        var rowsAffected = await _dbSession.Connection.ExecuteAsync(sql, orderItems, transaction: _dbSession.Transaction);
        
        return rowsAffected == orderItems.Count;
    }
}