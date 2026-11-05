using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.DAL.Models;

namespace ShopOrder.DAL.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetById(int id);
    }
}
