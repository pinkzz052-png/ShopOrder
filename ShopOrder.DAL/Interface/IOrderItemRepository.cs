using Microsoft.Data.SqlClient;
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Interface;

public interface IOrderItemRepository
{
    Task<bool> CreateOrderItemAsync(List<OrderItem> orderItem);
}