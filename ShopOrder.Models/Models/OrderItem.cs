using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOrder.Models.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Lấy từ JOIN để hiển thị
        public string? ProductName { get; set; }
    }
}
