using ShopOrder.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.BLL.DTOS;

namespace ShopOrder.BLL.Interface
{
    public interface IOrderServices
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto?> CreateAsync(CreateOrderRequest request);
        Task<OrderDto?> UpdateStatusAsync(int id, UpdateStatusOrderRequest status);
    }
}
