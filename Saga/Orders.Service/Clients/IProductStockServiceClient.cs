

namespace OrdersApi.Service.Clients
{
    public interface IProductStockServiceClient
    {
        Task<List<ProductStock>> GetStock(List<int> productIds);
    }
}