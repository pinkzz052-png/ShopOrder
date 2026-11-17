
using ShopOrder.BLL.DTOS;

namespace ShopOrder.BLL.Interface
{
    public interface ICustomerServices
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerRequest request);
        Task<CustomerDto> UpdateAsync(int id, UpdateCustomerRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
