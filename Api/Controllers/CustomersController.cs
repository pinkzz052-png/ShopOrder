using Microsoft.AspNetCore.Mvc;
using ShopOrder.BLL.DTOS;
using ShopOrder.BLL.Interface;

namespace ShopOrder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerServices _customerServices;

    public CustomersController(ICustomerServices customerServices)
    {
        _customerServices = customerServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var customers = await _customerServices.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerServices.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
    {
        var newCustomer = await _customerServices.CreateAsync(request);
        if (newCustomer == null) return BadRequest();
        return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] UpdateCustomerRequest request)
    {
        var updatedCustomer = await _customerServices.UpdateAsync(id, request);
        if (updatedCustomer == null) return BadRequest();
        return Ok(updatedCustomer);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomerAsync(int id)
    {
        var result = await _customerServices.DeleteAsync(id);
        if (!result) return BadRequest("Hiện chưa có chức năng xoa khach hang");
        return NotFound("Chức năng này chưa được thêm vào");
    }
}