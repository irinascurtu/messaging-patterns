using Contracts.Events;
using MassTransit;

namespace OrdersApi.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {


            var retries = context.GetRetryAttempt();
            if (retries > 0)
            {
                Console.WriteLine($"{retries}");
            }
            //throw new ArgumentNullException();
            await Task.Delay(1000);
            Console.WriteLine(context.ReceiveContext.InputAddress);
            Console.WriteLine($"I just consumed a message with OrderId{context.Message.OrderId}, " +
                $"that was created at:{context.Message.CreatedAt}");
        }
    }
}
