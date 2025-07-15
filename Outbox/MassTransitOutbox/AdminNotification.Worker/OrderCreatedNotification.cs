using Contracts.Events;
using MassTransit;
using Orders.Domain.Entities;
using Orders.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminNotification.Worker
{
    public class OrderCreatedNotification : IConsumer<OrderCreated>
    {
        private readonly IOrderService orderService;

        public OrderCreatedNotification(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {

            await context.Publish(new InvoiceNeeded()
            {
                Id = context.Message.Id,
                OrderId = context.Message.OrderId,
                TotalAmount = context.Message.TotalAmount,
                VAT = context.Message.TotalAmount * 1.19m
            });

            var existingOrder = await orderService.GetOrderAsync(context.Message.OrderId);
            if (existingOrder != null)
            {
                existingOrder.Status = OrderStatus.Created;
                await orderService.UpdateOrderAsync(existingOrder);
            }


            await Task.Delay(1000);
            Console.WriteLine(context.ReceiveContext.InputAddress);
            Console.WriteLine($"Admin Notification -I just consumed a message with OrderId {context.Message.OrderId}, " +
                $"that was created at:{context.Message.CreatedAt}");

            await Task.CompletedTask;
        }
    }
}
