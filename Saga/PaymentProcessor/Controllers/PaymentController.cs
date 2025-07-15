using Contracts.Events;
using Contracts.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Entities;
using PaymentProcessor.Services;

namespace PaymentProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;

        public PaymentController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost("/pay")]
        public async Task<ActionResult> Post(PayModel model)
        {
            await publishEndpoint.Publish(new OrderPaid()
            {
                AmountPaid = model.AmountPaid,
                OrderId = model.OrderId,
                PaymentMethod = model.PaymentMethod
            });
            return Ok("Payment Processor API");
        }

        [HttpGet("/requestcancelation/{id}")]
        public async Task<ActionResult<Order>> RequestCancelation(Guid id)
        {
            await publishEndpoint.Publish(new CancelationRequested()
            {
                OrderId = id
            });
            return Accepted();
        }
    }
}
