using Contracts.Response;
using MassTransit;
using Orders.Domain.Entities;
using Orders.Service;

namespace OrdersApi.Consumers
{
    public class VerifyOrderConsumer : IConsumer<VerifyOrder>
    {
        private readonly IOrderService orderService;

        public VerifyOrderConsumer(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task Consume(ConsumeContext<VerifyOrder> context)
        {
            var existingOrder = await orderService.GetOrderAsync(context.Message.Id);

            if (!context.IsResponseAccepted<Order>())
            {
                throw new ArgumentException(nameof(context));
            }

            if (existingOrder != null)
            {
                await context.RespondAsync<OrderResult>(new
                {
                    Id = context.Message.Id,
                    OrderDate = existingOrder.OrderDate,
                    Status = existingOrder.Status
                });
            }
            else
            {
                 await context.RespondAsync<OrderNotFoundResult>(new OrderNotFoundResult()
                 {
                     ErrorMessage="Sorry your order is lost! or not here"
                 });
            }


            //check for an existing order
           
        }
    }
}
