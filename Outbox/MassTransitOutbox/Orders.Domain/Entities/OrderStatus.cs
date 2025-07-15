using System.ComponentModel;

namespace Orders.Domain.Entities
{
    public enum OrderStatus
    {
        [Description("Received")]//think about these as temporary orders
        Received,
        [Description("Created")]
        Created,

        //after a while, the status becomes Pending - 5 mins or so
        [Description("Pending")]
        Pending,

        [Description("Cancelled")]
        Cancelled,
        //PayOrder command is sent
        //OrderPayed event is sent
        [Description("Paid")]
        Paid,

        //time-boxed- if not paid in x mins, becomes awaiting payment
        [Description("AwaitingPayment")]
        AwaitingPayment,

        //listens to OrderPaid event, and changes status to ReadyForShipping
        [Description("ReadyForShipping")]
        ReadyForShipping,

        [Description("Shipped")]
        Shipped,

        [Description("Delivered")]
        Delivered,

        [Description("Completed")]
        Completed

    }
}
