using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopOrder.BLL.DTOS;
using ShopOrder.BLL.Interface;
using System.Net.WebSockets;

namespace ShopOrder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductAsync()
        {
            var products = await _productServices.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productServices.GetByIdAsync(id);
            if (product is null)
                return NotFound();

            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto dto)
        {
            var createdProduct = await _productServices.CreateAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestDto dto)
        {
            var updatedProduct = await _productServices.UpdateAsync(dto);
            return Ok(updatedProduct);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _productServices.DeleteAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
