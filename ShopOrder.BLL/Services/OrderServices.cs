using ShopOrder.BLL.DTOS;
using ShopOrder.BLL.Interface;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;
using Mapster;

namespace ShopOrder.BLL.Services;

public class OrderServices : IOrderServices
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderServices(IOrderRepository orderRepository, IProductRepository productRepository, IOrderItemRepository orderItemRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Adapt<List<OrderDto>>();
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null) return null;
        return order.Adapt<OrderDto>();
    }

    public async Task<int?> CreateAsync(CreateOrderRequest request)
    {
        if (request.Items.Count == 0)
        {
            throw new Exception("Items cannot be empty");
        }

        var groupItems = request.Items
            .GroupBy(p => p.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        if (groupItems.Any(x => x.Quantity == 0)) 
            throw new Exception("So luong phai lon hon khong");
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var productIds = groupItems.Select(x => x.ProductId).Distinct();
            var products = await _productRepository.GetByIdsAsync(productIds);
            var productDict = products.ToDictionary(k => k.Id);

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                if (!productDict.TryGetValue(item.ProductId, out var product))
                    throw new InvalidOperationException($"Product {item.ProductId} doesn't exist");

                var stockUpdated = await _productRepository.DecreaseStockQuantityAsync(item.ProductId, item.Quantity);
                if (!stockUpdated)
                    throw new InvalidOperationException("khong du so luong");

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                });

                totalAmount += item.Quantity * product.Price;
            }

            var order = new Order
            {
                CustomerId = request.CustomerId,
                TotalAmount = totalAmount,
                Note = request.Note
            };
            int newOrderId = await _orderRepository.CreateAsync(order);

            foreach (var item in orderItems)
            {
                item.OrderId = newOrderId;
            }

            var createdOrderItems = await _orderItemRepository.CreateOrderItemAsync(orderItems);
            if (!createdOrderItems) throw new InvalidOperationException("khong the tao don hang");

            await _unitOfWork.CommitAsync();
            return newOrderId;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateStatusOrderRequest status)
    {
        var statusString = Convert.ToString(status.status);
        var updatedOrder = await _orderRepository.UpdateStatusAsync(id, statusString);
        if (updatedOrder == null) return null;
        return updatedOrder.Adapt<OrderDto>();
    }
}