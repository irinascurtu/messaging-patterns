using Contracts.Commands;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Worker
{
    public class RefundOrderConsumer : IConsumer<RefundOrder>
    {
        public Task Consume(ConsumeContext<RefundOrder> context)
        {
            Console.WriteLine($"Got a refund order:{context.Message.OrderId}");

            return Task.CompletedTask;
        }
    }
}
