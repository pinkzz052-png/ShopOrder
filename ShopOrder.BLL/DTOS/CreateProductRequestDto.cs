using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopOrder.BLL.DTOS
{
    public class CreateProductRequestDto
    {
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;
        [Range(0, 1_000_000_000)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
    }
}
