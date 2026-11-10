using System;
using System.Collections.Generic;
using System.Text;

namespace ShopOrder.DAL.Interface
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T> CreateAsync(T entity);
    }
}
