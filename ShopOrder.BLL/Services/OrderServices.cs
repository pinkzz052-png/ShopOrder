using ShopOrder.BLL.Interface;
using ShopOrder.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.Models.Models;
using ShopOrder.BLL.DTOS;
using System.Net.WebSockets;
using System.Diagnostics;
namespace ShopOrder.BLL.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        public OrderServices(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => MapToOrderDto(o)).ToList();
        }
        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order is null) return null;
            return MapToOrderDto(order);
        }
        public async Task<OrderDto?> CreateAsync(CreateOrderRequest request)
        {
            
            var newOrder = await _orderRepository.CreateAsync(MapDtoToOrder(request));
            if (newOrder == null) return null;
            return MapToOrderDto(newOrder);
        }
        public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateStatusOrderRequest status)
        {
            string statusString = Convert.ToString(status.status);
            var updatedOrder = await _orderRepository.UpdateStatusAsync(id, statusString);
            if (updatedOrder == null) return null;
            return MapToOrderDto(updatedOrder);
        }
        public static OrderDto MapToOrderDto(Order order) => new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Note = order.Note,
            CustomerName = order.CustomerName,
            Items = order.Items.Select(it => MapToOrderItemDto(it)).ToList()
        };
        public static Order MapDtoToOrder(CreateOrderRequest request) => new Order
        {
            CustomerId = request.CustomerId,
            TotalAmount = request.TotalAmount,
            Note = request.Note is null ? null : request.Note,
            Items = request.Items
        };
        public static OrderItemDto MapToOrderItemDto(OrderItem orderItem) => new OrderItemDto
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            ProductName = orderItem.ProductName
        };
    }
}
