using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.BLL.Interface;
using ShopOrder.DAL.Interface;
using ShopOrder.BLL.DTOS;
using ShopOrder.Models.Models;

namespace ShopOrder.BLL.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => MapToCategoryDto(c)).ToList();
        }
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }
            return MapToCategoryDto(category);
        }
        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
        {
            var category = new Category
            {
                Name = request.Name
            };
            var createdCategory = await _categoryRepository.CreateAsync(category);
            return MapToCategoryDto(createdCategory);
        }
        public async Task<CategoryDto> UpdateAsync(int id,UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.UpdateAsync(id, new Category
            {
                Name = request.Name
            });
            return MapToCategoryDto(category);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        public static CategoryDto MapToCategoryDto(Category category) => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
