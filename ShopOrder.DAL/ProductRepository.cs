using ShopOrder.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.DAL.Models;
using Microsoft.Data.SqlClient;

namespace ShopOrder.DAL
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
    }
}
