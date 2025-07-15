
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace Stocks.Repository
{
    public class ProductStockRepository : IProductStockRepository
    {
        private readonly StockContext productContext;

        public ProductStockRepository(StockContext productContext)
        {
            this.productContext = productContext;
        }
        public async Task<IEnumerable<ProductStock>> GetProductStocksAsync(List<int> productIds)
        {
            // Generate the SQL parameter placeholders (e.g., @p0, @p1, @p2)
            var parameters = productIds.Select((id, index) => $"@p{index}").ToArray();
            var inClause = string.Join(",", parameters);

            // Construct the raw SQL query
            var sql = $"SELECT Id as ProductId, Stock FROM Products WHERE Id IN ({inClause})";

            // Generate the SqlParameter array
            var sqlParameters = productIds.Select((id, index) => new SqlParameter($"@p{index}", id)).ToArray();

            // Execute the raw SQL query
            var stocks = productContext.ProductStocks
                                    .FromSqlRaw(sql, sqlParameters)
                                    .ToList();

            return stocks;
        }
    }
}
