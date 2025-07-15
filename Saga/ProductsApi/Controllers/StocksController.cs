using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Service;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IProductService productService;

        public StocksController(IProductService productService)
        {
            this.productService = productService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsStocks([FromQuery] List<int> productIds)
        {
            // Create a Random object
            Random random = new Random();

            // Generate a random number between 0 and 100
            int number = random.Next(0, 101);
            if (number % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("BOOM! A failure occurred");
            }
         
            var products = await productService.GetProductsStocksAsync(productIds);
            return Ok(products);
            ///TODO: add mappings and use the Model
            ///
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStocks([FromQuery] Dictionary<int, int> productStocks)
        {
            var products = await productService.UpdateProductStocks(productStocks);
            return Ok(products);
            ///TODO: add mappings and use the Model
            ///
        }
    }
}
