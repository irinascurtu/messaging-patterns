using MassTransit;

namespace OrdersApi.Consumers
{
    public class AllFaultsConsumer : IConsumer<Fault>
    {
        public Task Consume(ConsumeContext<Fault> context)
        {
            Console.WriteLine("All the faults that I listen to. Lalalalal!");
           return Task.CompletedTask;
        }
    }
}
