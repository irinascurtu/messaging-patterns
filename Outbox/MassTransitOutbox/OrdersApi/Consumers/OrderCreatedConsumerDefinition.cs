using MassTransit;

namespace OrdersApi.Consumers
{
    public class OrderCreatedConsumerDefinition : ConsumerDefinition<OrderCreatedConsumer>
    {
        public OrderCreatedConsumerDefinition()
        {
            Endpoint(e =>
            {
                e.ConcurrentMessageLimit = 10;
            });
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<OrderCreatedConsumer> consumerConfigurator,
            IRegistrationContext context)
        {

            endpointConfigurator.PublishFaults = false;
            consumerConfigurator.UseMessageRetry(r =>
            {
                //r.Immediate(2);
                //r.Exponential(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(40), TimeSpan.FromSeconds(10));
                r.Intervals(
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(30),
                    TimeSpan.FromSeconds(2));

            });
        }
    }
}
