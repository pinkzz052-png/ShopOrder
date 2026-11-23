
using ShopOrder.DAL.Interface;
using ShopOrder.BLL.DTOS;
using ShopOrder.Models.Models;

using ShopOrder.BLL.Interface;

namespace ShopOrder.BLL.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        public ProductServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => ToDto(p)).ToList();
        }
        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product is not null ? ToDto(product) : null;
        }
        public async Task<ProductDto?> CreateAsync(CreateProductRequestDto dto)
        {
            var product = new Product
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
            };
            var newProduct = await _productRepository.CreateAsync(product);
            return ToDto(newProduct);
        }
        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductRequestDto dto)
        {
            var product = new Product
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
            };
            var updateProduct = await _productRepository.UpdateAsync(id, product);
            return ToDto(updateProduct);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public static ProductDto ToDto(Product product) => new ProductDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}
