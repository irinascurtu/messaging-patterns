using Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Worker
{
    public class CancelationRequestedConsumer : IConsumer<CancelationRequested>
    {
        public Task Consume(ConsumeContext<CancelationRequested> context)
        {
            Console.WriteLine($"CancelationRequested consumer called for orderId :{context.Message.OrderId}");
            //update the order status on then order itself

            return Task.CompletedTask;
        }
    }
}
