using Microsoft.AspNetCore.Mvc;
using ShopOrder.DAL;

namespace ShopOrder.Api.Controllers
{
    public class TestController : Controller
    {
        private readonly IGetConnectionString _getConnectionString;
        public TestController(IGetConnectionString getConnectionString)
        {
            _getConnectionString = getConnectionString;
        }
        public IActionResult Index()
        {
            string? result = _getConnectionString.GetConnectionStringMethod();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
