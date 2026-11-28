using ShopOrder.BLL.DTOS;
using ShopOrder.BLL.Interface;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;

namespace ShopOrder.BLL.Services;

public class CustomerServices : ICustomerServices
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerServices(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers.Select(c => MapToCustomerDto(c)).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null) return null;
        return MapToCustomerDto(customer);
    }

    public async Task<CustomerDto?> CreateAsync(CreateCustomerRequest request)
    {
        var customer = new Customer
        {
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address
        };
        var newCustomer = await _customerRepository.CreateAsync(customer);
        return MapToCustomerDto(newCustomer);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = new Customer
        {
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address
        };
        var updatedCustomer = await _customerRepository.UpdateAsync(id, customer);
        return MapToCustomerDto(updatedCustomer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return false;
    }

    public static CustomerDto MapToCustomerDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address
        };
    }
}