using Contracts.Commands;
using Contracts.Events;
using MassTransit;
using Orders.Domain.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSaga.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateData>
    {
        public State Created { get; set; }
        public State Pending { get; set; }
        public State Paid { get; set; }
        public State Canceled { get; set; }
        public State Completed { get; set; }
        public State AwaitingPayment { get; set; }


        public Event<OrderCreated> OrderCreated { get; set; }
        public Event<OrderPaid> OrderPaid { get; set; }
        public Event<CancelationRequested> CancelationRequested { get; set; }
        public Event<CancelOrder> CancelOrder { get; set; }
        public Event<OrderCompleted> OrderCompleted { get; set; }

        public Event PaymentTimeout { get; set; }

        public Schedule<OrderStateData, PaymentTimeoutExpired> PaymentTimeoutSchedule { get; set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreated, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderPaid, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => CancelationRequested, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => CancelOrder, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderCompleted, x => x.CorrelateById(context => context.Message.OrderId));

            Schedule(() => PaymentTimeoutSchedule, x => x.PaymentTimeoutTokenId,
                s =>
                {
                    s.Delay = TimeSpan.FromSeconds(30);
                    s.Received = e => e.CorrelateById(context => context.Message.OrderId);
                });


            Initially(
                When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.Amount = context.Message.TotalAmount;
                    context.Saga.OrderStatus = Orders.Domain.Entities.OrderStatus.Pending;
                    context.Saga.CreatedAt = context.Message.CreatedAt;
                    Console.WriteLine($"OrderCreated{context.Message.OrderId}");
                })
                .TransitionTo(Pending)
                .Schedule(PaymentTimeoutSchedule, context =>
                    new PaymentTimeoutExpired { OrderId = context.Saga.OrderId })
                );

            During(Pending,
                When(OrderPaid)
                    .Then(context =>
                    {
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.Amount = context.Message.AmountPaid;
                        context.Saga.PaidAt = DateTime.UtcNow;
                        context.Saga.IsBilled = false;
                        Console.WriteLine($"Paid the order {context.Message.OrderId} of {context.Message.AmountPaid} paid");
                    })
                    .TransitionTo(Paid)
                    .Publish(context => new InvoiceNeeded()
                    {
                        TotalAmount = context.Saga.Amount,
                        VAT = context.Saga.Amount * 1.19m,
                        OrderId = context.Message.OrderId,//send an invoice 
                    }),
                When(PaymentTimeoutSchedule.Received)
                    .Then(context =>
                    {
                        Console.WriteLine("Payment timeout occurred, Moving to AwaitingPayment");
                    }).TransitionTo(AwaitingPayment),

                When(CancelationRequested)
                    .Then(context =>
                    {
                        Console.WriteLine("Cancelation requested");
                        context.Saga.CanceledAt = DateTime.UtcNow;
                        context.Saga.OrderStatus = Orders.Domain.Entities.OrderStatus.CancelationRequested;
                    })
                 .TransitionTo(Canceled)
                 .Publish(context => new OrderCanceled
                 {
                     OrderId = context.Saga.OrderId
                 })
                  );

            During(Paid,
              When(CancelationRequested)
                  .Then(async context =>
                  {
                      context.Saga.OrderId = context.Message.OrderId;
                      context.Saga.PaidAt = DateTime.UtcNow;
                      context.Saga.IsBilled = false;
                      Console.WriteLine($"Canceling the order {context.Message.OrderId}");

                      await context.Publish(new OrderCanceled()
                      {
                          OrderId = context.Saga.OrderId
                      });

                  })
                  .TransitionTo(Canceled)
                  .Publish(context => new RefundOrder
                  {
                      OrderId = context.Saga.OrderId
                  })
                  );

            During(Canceled,
                Ignore(CancelationRequested),
                Ignore(OrderPaid)

                );

            //DuringAny(When(OrderCompleted).Finalize());
            //SetCompletedWhenFinalized();
        }
    }
}
