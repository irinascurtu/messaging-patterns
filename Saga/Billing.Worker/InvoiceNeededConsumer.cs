using Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminNotification.Worker
{
    public class InvoiceNeededConsumer : IConsumer<Contracts.Events.InvoiceNeeded>
    {
        public Task Consume(ConsumeContext<InvoiceNeeded> context)
        {
            Console.WriteLine($"Invoice needed!!{context.MessageId}");
            return Task.CompletedTask;
        }
    }
}
