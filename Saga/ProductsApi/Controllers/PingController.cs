using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiVersionNeutral]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET: api/
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "pong", "ping" };
        }
    }
}
