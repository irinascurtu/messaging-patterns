using ProductsApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ProductsApi.Controllers
{
    // [ApiVersion("2")]

    [Asp.Versioning.ApiVersion("2")]
    [Asp.Versioning.AdvertiseApiVersions("2")]
    [ApiExplorerSettings(GroupName = "v2")]

    //[Route("api/v2/products")]
    [Route("api/products")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductsController> logger;

        public ProductsV2Controller(IProductService productService, ILogger<ProductsController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        [HttpGet]
        [Produces("application/vnd.example.v2+json")]
        [EnableRateLimiting("concurrency")]

        public async Task<IActionResult> GetProducts()
        {
            var products = await productService.GetProductsAsync();
            return Ok("this is V2");
            ///TODO: add mappings and use the Model
            ///
        }
    }
}
