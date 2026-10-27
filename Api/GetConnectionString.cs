using ShopOrder.DAL;

namespace ShopOrder.Api
{
    public class GetConnectionString : IGetConnectionString
    {
        IConfiguration Configuration;
        public string? GetConnectionStringMethod()
        {
            string? connectionString = Configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
        public GetConnectionString(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
