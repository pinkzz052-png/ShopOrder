using ShopOrder.BLL.DTOS;

namespace ShopOrder.BLL.Interface;

public interface IOrderServices
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<int?> CreateAsync(CreateOrderRequest request);
    Task<OrderDto?> UpdateStatusAsync(int id, UpdateStatusOrderRequest status);
}