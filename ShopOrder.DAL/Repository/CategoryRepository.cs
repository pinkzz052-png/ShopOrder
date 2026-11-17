using Microsoft.Data.SqlClient;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace ShopOrder.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public CategoryRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            const string sql = @"
                SELECT Id, Name FROM Categories
                ORDER BY Id;";

            var categorys = new List<Category>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);

            await using var reader = await command.ExecuteReaderAsync();

            int idCol = reader.GetOrdinal("Id");
            int nameCol = reader.GetOrdinal("Name");

            while (await reader.ReadAsync())
            {
                categorys.Add(new Category
                {
                    Id = reader.GetInt32(idCol),
                    Name = reader.GetString(nameCol)
                });
            }

            return categorys;
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, Name FROM Categories
                WHERE Categories.Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                };
            }
            throw new InvalidOperationException("Not found");
        }
        public async Task<Category> CreateAsync(Category newCategory)
        {
            const string sql = @"
                INSERT INTO Categories (Name)
                OUTPUT INSERTED.Id, INSERTED.Name
                VALUES (@Name)";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = newCategory.Name;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException("Can not create new Category");
            }
            return ToCategory(reader);
        }
        public async Task<Category> UpdateAsync(int id, Category category)
        {
            const string sql = @"
                UPDATE Categories
                SET Name = @Name
                OUTPUT INSERTED.Id, INSERTED.Name
                WHERE Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = category.Name;
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException("Can not update Category");
            }
            return ToCategory(reader);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                DELETE FROM Categories
                WHERE Categories.Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            return true;
        }

        public static Category ToCategory(SqlDataReader reader) => new Category
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name"))
        };
    }
}
