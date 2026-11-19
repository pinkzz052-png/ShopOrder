
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task<Order?> UpdateStatusAsync(int id, string status);
    }
}
