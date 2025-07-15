using Contracts.Events;
using MassTransit;
using Orders.Service;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdminNotification
{

    public class OrderReceivedConsumer : IConsumer<OrderReceived>
    {
        private readonly IOrderService orderService;

        public OrderReceivedConsumer(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task Consume(ConsumeContext<OrderReceived> context)
        {
            Console.WriteLine(context.ReceiveContext.InputAddress);
            Console.WriteLine("OrderReceived Event: I'll jump!!");
            throw new Exception("Bad order");
            string jsonString = JsonSerializer.Serialize(context.Message, new JsonSerializerOptions
            {
                WriteIndented = true,

            });

            Console.WriteLine(jsonString);

        }
    }
}
