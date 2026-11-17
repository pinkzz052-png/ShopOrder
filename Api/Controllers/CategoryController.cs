using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopOrder.BLL.DTOS;
using System.Net.WebSockets;
using ShopOrder.BLL.Interface;


namespace ShopOrder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var categories = await _categoryServices.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryServices.GetByIdAsync(id);
            if (category is null)
                return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var createdCategory = await _categoryServices.CreateAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            var updatedCategory = await _categoryServices.UpdateAsync(id, request);
            return Ok(updatedCategory);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryServices.DeleteAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
