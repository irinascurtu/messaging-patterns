using ProductsApi.Data.Entities;
using ProductsApi.Data.Extensions;
using ProductsApi.Data.Repositories;

namespace ProductsApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<IEnumerable<ProductStock>> GetProductsStocksAsync(List<int> productsIds)
        {
            return await _productRepository.GetProductStocksAsync(productsIds);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _productRepository.GetProductAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            return await _productRepository.AddProductAsync(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepository.ProductExistsAsync(id);
        }

        public async Task<IEnumerable<ProductStock>> UpdateProductStocks(Dictionary<int, int> productStocks)
        { 
         
         return await _productRepository.UpdateProductStocks(productStocks);
        }
    }
}
