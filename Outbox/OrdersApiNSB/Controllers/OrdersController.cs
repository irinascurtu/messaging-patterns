using Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;


namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersApiNSB.Services.IOrderServiceNSB _orderService;

        public OrdersController(OrdersApiNSB.Services.IOrderServiceNSB orderService)
        {
            _orderService = orderService;

        }


        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderModel model)
        {

            await _orderService.AcceptOrder(model);

            return Accepted();
        }

    }
}
