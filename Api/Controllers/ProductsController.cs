using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopOrder.BLL;

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
    }
}
