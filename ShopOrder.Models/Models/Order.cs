using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOrder.Models.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }

        // Không phải cột của bảng Orders: lấy từ JOIN để hiển thị
        public string? CustomerName { get; set; }

        // Các dòng hàng của đơn (bảng OrderItems)
        public List<OrderItem> Items { get; set; } = new();
    }
}
