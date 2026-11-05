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

        public async Task<List<Product>> GetAllAsync()
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
        public async Task<Product?> GetById(int id)
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
