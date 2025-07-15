using Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Worker
{
    public class OrderCanceledConsumer : IConsumer<OrderCanceled>
    {
        public Task Consume(ConsumeContext<OrderCanceled> context)
        {
            Console.WriteLine($"The order with orderId :{context.Message.OrderId} was canceled.OrderCanceled event");
            context.Publish(new OrderCompleted { 
                OrderId = context.Message.OrderId
            });
            return Task.CompletedTask;
        }
    }
}
