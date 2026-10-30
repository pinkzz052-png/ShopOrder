using Microsoft.AspNetCore.Mvc;
using ShopOrder.DAL;

namespace ShopOrder.Api.Controllers
{
    public class TestController : Controller
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public TestController(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
    }
}
