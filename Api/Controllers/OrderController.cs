using Microsoft.AspNetCore.Mvc;
using ShopOrder.BLL.DTOS;
using ShopOrder.BLL.Interface;

namespace ShopOrder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrderAsync()
        {
            var orders = await _orderServices.GetAllAsync();
            return Ok(orders);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var order = await _orderServices.GetByIdAsync(id);
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            var newOrder = await _orderServices.CreateAsync(request);
            return Ok(newOrder);
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateStatusOrderAsync(int id, [FromBody] UpdateStatusOrderRequest status)
        {
            var updatedOrder = await _orderServices.UpdateStatusAsync(id, status);
            return Ok(updatedOrder);
        }
    }
}
