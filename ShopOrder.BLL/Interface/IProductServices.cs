using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.BLL.DTOS;

namespace ShopOrder.BLL.Interface
{
    public interface IProductServices
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductRequestDto requestDto);
        Task<ProductDto> UpdateAsync(UpdateProductRequestDto requestDto);
        Task<bool> DeleteAsync(int id);
    }
}
