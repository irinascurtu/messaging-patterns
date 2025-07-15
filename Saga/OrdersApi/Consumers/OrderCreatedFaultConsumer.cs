using Contracts.Events;
using MassTransit;

namespace OrdersApi.Consumers
{
    public class OrderCreatedFaultConsumer : IConsumer<Fault<OrderCreated>>
    {
        public Task Consume(ConsumeContext<Fault<OrderCreated>> context)
        {
            Console.WriteLine($"This is a OrderCreatedFault. The message faulted {context.Message.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
