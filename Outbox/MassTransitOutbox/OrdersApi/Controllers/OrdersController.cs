using Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using Orders.Service;

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
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
