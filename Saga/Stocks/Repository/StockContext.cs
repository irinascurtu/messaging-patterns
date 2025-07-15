using Microsoft.EntityFrameworkCore;

namespace Stocks.Repository
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<ProductStock> ProductStocks { get; set; }
    }
}
