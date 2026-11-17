using ShopOrder.BLL.DTOS;


namespace ShopOrder.BLL.Interface
{
    public interface ICategoryServices
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
        Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
