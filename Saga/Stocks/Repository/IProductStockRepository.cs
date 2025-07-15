namespace Stocks.Repository
{
    public interface IProductStockRepository
    {
        Task<IEnumerable<ProductStock>> GetProductStocksAsync(List<int> productIds);
    }
}
