using Microsoft.AspNetCore.Mvc;
using ShopOrder.BLL.DTOS;
using ShopOrder.BLL.Interface;

namespace ShopOrder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        public OrdersController(IOrderServices orderServices)
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
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            var newOrder = await _orderServices.CreateAsync(request);
            if (newOrder == null) return BadRequest();
            return Ok(newOrder);
        }
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateStatusOrderAsync(int id, [FromBody] UpdateStatusOrderRequest status)
        {
            var updatedOrder = await _orderServices.UpdateStatusAsync(id, status);
            if (updatedOrder == null) return BadRequest();
            return Ok(updatedOrder);
        }
    }
}
