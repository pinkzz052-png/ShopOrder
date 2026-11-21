using ShopOrder.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOrder.BLL.DTOS
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public string? Note { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }
}
