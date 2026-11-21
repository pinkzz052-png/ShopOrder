using ShopOrder.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOrder.BLL.DTOS
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }

        public string? CustomerName { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }
}
