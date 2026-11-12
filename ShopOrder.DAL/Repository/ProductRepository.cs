using ShopOrder.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.DAL.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ShopOrder.DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ProductRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            const string sql = @"
                SELECT Id, CategoryId, Name, Price, StockQuantity, IsActive, CreatedAt
                FROM Products  
                ORDER BY Id;";
  
            var products = new List<Product>();

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            int idCol = reader.GetOrdinal("Id");
            int categoryIdCol = reader.GetOrdinal("CategoryId");
            int nameCol = reader.GetOrdinal("Name");
            int priceCol = reader.GetOrdinal("Price");
            int stockCol = reader.GetOrdinal("StockQuantity");
            int isActiveCol = reader.GetOrdinal("IsActive");
            int createdAtCol = reader.GetOrdinal("CreatedAt");

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(idCol),
                    CategoryId = reader.GetInt32(categoryIdCol),
                    Name = reader.GetString(nameCol),
                    Price = reader.GetDecimal(priceCol),
                    StockQuantity = reader.GetInt32(stockCol),
                    IsActive = reader.GetBoolean(isActiveCol),
                    CreatedAt = reader.GetDateTime(createdAtCol)
                });
            }

            return products;
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, CategoryId, Name, Price, StockQuantity, IsActive, CreatedAt
                FROM Products
                WHERE Id = @Id;";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapToProduct(reader);
        }
        public async Task<Product> CreateAsync(Product product)
        {
            const string sql = @"
                INSERT INTO Products (CategoryId, Name, Price, StockQuantity)
                OUTPUT INSERTED.Id, INSERTED.CategoryId, INSERTED.Name, INSERTED.Price, INSERTED.StockQuantity, INSERTED.IsActive, INSERTED.CreatedAt
                VALUES(@CategoryId, @Name, @Price, @StockQuantity)
                ";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = product.Name;
            var price = command.Parameters.Add("@Price", SqlDbType.Decimal);
            price.Precision = 18;
            price.Scale = 2;
            price.Value = product.Price;
            command.Parameters.Add("@StockQuantity", SqlDbType.Int).Value = product.StockQuantity;

            await using var reader = await command.ExecuteReaderAsync();
            
            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException("Failed to create product.");
            }
            return MapToProduct(reader);
        }
        public async Task<Product> UpdateAsync(int id, Product product)
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

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = product.Name;
            var price = command.Parameters.Add("@Price", SqlDbType.Decimal);
            price.Precision = 18;
            price.Scale = 2;
            price.Value = product.Price;
            command.Parameters.Add("@StockQuantity", SqlDbType.Int).Value = product.StockQuantity;
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException("Fail to update product. Product not found.");
            }
            return MapToProduct(reader);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            const string sql= @"
                Update Products
                SET IsActive = 0
                WHERE Id = @Id;";

            await using var connection = _dbConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();
            return true;
        }


        // Map dữ liệu từ reader sang đối tượng Product
        public static Product MapToProduct(SqlDataReader reader) => new Product
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
            StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
        };
    }
}
