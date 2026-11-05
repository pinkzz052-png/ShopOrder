using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.DAL.Interface;
using ShopOrder.BLL.DTOS;
using ShopOrder.DAL.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace ShopOrder.BLL
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
        public async Task<ProductDto?> GetById(int id)
        {
            var product = await _productRepository.GetById(id);
            return product is not null ? ToDto(product) : null;
        }

        public static ProductDto ToDto(Product product) => new ProductDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };
    }
}
