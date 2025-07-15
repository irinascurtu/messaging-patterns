using System.Net.Http.Json;
using System.Web;

namespace OrdersApi.Service.Clients
{
    public class ProductStockServiceClient : IProductStockServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductStockServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7221");//localhost something
        }

        public async Task<List<ProductStock>> GetStock(List<int> productIds)
        {  // Build the query string
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var productId in productIds)
            {
                queryString.Add("productIds", productId.ToString());
            }

            // call the client to post something and 
            var response = await _httpClient.GetFromJsonAsync<List<ProductStock>>($"api/stocks?{queryString}");


            ///see what it there to do
            return response;
        }

    }
}
