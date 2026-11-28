namespace ShopOrder.BLL.DTOS;

public class UpdateProductRequestDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}