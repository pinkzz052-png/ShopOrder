using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbSession _dbSession;

    public CategoryRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        const string sql = @"
                SELECT Id, Name FROM Categories
                ORDER BY Id;";

        return await _dbSession.Connection.QueryAsync<Category>(sql, transaction: _dbSession.Transaction);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        const string sql = @"
                SELECT Id, Name FROM Categories
                WHERE Categories.Id = @Id";

        var category = await _dbSession.Connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        if (category == null) return null;
        return category;
    }

    public async Task<Category?> CreateAsync(Category newCategory)
    {
        const string sql = @"
                INSERT INTO Categories (Name)
                OUTPUT INSERTED.Id, INSERTED.Name
                VALUES (@Name)";

        var category = await _dbSession.Connection.QuerySingleAsync<Category>(sql, newCategory, transaction: _dbSession.Transaction);
        return category;
    }

    public async Task<Category?> UpdateAsync(int id, Category category)
    {
        const string sql = @"
                UPDATE Categories
                SET Name = @Name
                OUTPUT INSERTED.Id, INSERTED.Name
                WHERE Id = @Id";

       var updatedCategory =  await _dbSession.Connection.ExecuteScalarAsync<Category>(sql, category);
       if (updatedCategory == null) return null;
       return updatedCategory;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
                DELETE FROM Categories
                WHERE Categories.Id = @Id";

        var rowsAffected = await _dbSession.Connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}