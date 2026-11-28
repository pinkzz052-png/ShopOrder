using Microsoft.Data.SqlClient;
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Interface;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> DecreaseStockQuantityAsync(int productId, int quantity);
    Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids);
}