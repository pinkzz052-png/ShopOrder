using System;
using System.Collections.Generic;
using System.Text;
using ShopOrder.BLL.DTOS;

namespace ShopOrder.BLL
{
    public interface IProductServices
    {
        Task<List<ProductDto>> GetAllAsync();
    }
}
