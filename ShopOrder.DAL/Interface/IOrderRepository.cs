using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Interface;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<int> CreateAsync(Order order);
    Task<Order?> UpdateStatusAsync(int id, string status);
}