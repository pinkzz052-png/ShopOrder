
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;
using Dapper;

namespace ShopOrder.DAL.Repository;

public class ProductRepository : IProductRepository
{
    private readonly IDbSession _dbSession;

    public ProductRepository(
    IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        const string sql = @"
                SELECT Id, CategoryId, Name, Price, StockQuantity, IsActive, CreatedAt
                FROM Products  
                WHERE IsActive = 1
                ORDER BY Id;";
        
        return _dbSession.Connection.Query<Product>(sql, transaction: _dbSession.Transaction);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        const string sql = @"
                SELECT Id, CategoryId, Name, Price, StockQuantity, IsActive, CreatedAt
                FROM Products
                WHERE Id = @Id AND IsActive = 1;";

        return await _dbSession.Connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id },
            transaction: _dbSession.Transaction);
    }
    
    public async Task<List<Product>> GetByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.ToList();
        const string sql = @"
            SELECT Id, Price, StockQuantity, IsActive
            FROM Products
            WHERE Id IN @Ids AND IsActive = 1;";

        var result = await _dbSession.Connection.QueryAsync<Product>(sql, new { Ids = idList },
            transaction: _dbSession.Transaction);
        return result.ToList();
    }
    public async Task<Product?> CreateAsync(Product product)
    {
        const string sql = @"
                INSERT INTO Products (CategoryId, Name, Price, StockQuantity)
                OUTPUT INSERTED.Id, INSERTED.CategoryId, INSERTED.Name, INSERTED.Price, INSERTED.StockQuantity, INSERTED.IsActive, INSERTED.CreatedAt
                VALUES(@CategoryId, @Name, @Price, @StockQuantity);
                ";

        return await _dbSession.Connection.QuerySingleAsync<Product>(sql, product, transaction: _dbSession.Transaction);
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        const string sql = @"
                UPDATE Products
                SET CategoryId = @CategoryId,
                    Name = @Name,
                    Price = @Price,
                    StockQuantity = @StockQuantity
                OUTPUT INSERTED.Id, INSERTED.CategoryId, INSERTED.Name, INSERTED.Price, INSERTED.StockQuantity, INSERTED.IsActive, INSERTED.CreatedAt
                WHERE Id = @Id;
                ";

        return await _dbSession.Connection.QuerySingleAsync<Product>(sql, new { Id = id, product },
            transaction: _dbSession.Transaction);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
                Update Products
                SET IsActive = 0
                WHERE Id = @Id;";

        var rowAffected = await _dbSession.Connection.ExecuteAsync(sql, new { Id = id }, transaction: _dbSession.Transaction);
        
        if (rowAffected == 0) throw new InvalidOperationException("Fail to delete product. Product not found.");

        return true;
    }

    public async Task<bool> DecreaseStockQuantityAsync(int productId, int quantity)
    {
        const string sql = @"
                UPDATE Products
                SET StockQuantity = StockQuantity - @Quantity
                WHERE Id = @Id AND StockQuantity >= @Quantity;";

        var rowAffected = await _dbSession.Connection.ExecuteAsync(sql, new { Id = productId, Quantity = quantity }, transaction: _dbSession.Transaction);
            if (rowAffected == 0) throw new InvalidOperationException("Fail to update product. Product not found.");
            
        return rowAffected > 0;
    }
}