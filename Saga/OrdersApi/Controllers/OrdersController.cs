using AutoMapper;
using Contracts.Events;
using Contracts.Filters;
using Contracts.Models;
using Contracts.Response;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using Orders.Service;
using OrdersApi.Service.Clients;

namespace OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductStockServiceClient productStockServiceClient;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly IRequestClient<VerifyOrder> requestClient;
        private readonly Tenant tenant;

        public OrdersController(IOrderService orderService,
            IProductStockServiceClient productStockServiceClient,
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            ISendEndpointProvider sendEndpointProvider,
            IRequestClient<VerifyOrder> requestClient,
            Tenant tenant
            )
        {
            _orderService = orderService;
            this.productStockServiceClient = productStockServiceClient;
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
            this.sendEndpointProvider = sendEndpointProvider;
            this.requestClient = requestClient;
            this.tenant = tenant;
            this.tenant.TenantId = Guid.NewGuid().ToString();
        }


        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderModel model)
        {

            await _orderService.AcceptOrder(model);

            return Accepted();
        }


        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var response = await requestClient.GetResponse<OrderResult, OrderNotFoundResult, Order>(new VerifyOrder()
            {
                Id = id
            });

            if (response.Is(out Response<OrderResult> incomingMessage))
            {
                return Ok(incomingMessage.Message);
            }


            if (response.Is(out Response<OrderNotFoundResult> notFound))
            {
                return NotFound(notFound.Message);
            }

            //var order = await _orderService.GetOrderAsync(id);
            //if (order == null)
            //{
            //    return NotFound();
            //}

            //return Ok(order);

            return BadRequest();
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            try
            {
                await _orderService.UpdateOrderAsync(order);
            }
            catch
            {
                if (!await _orderService.OrderExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }



        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
