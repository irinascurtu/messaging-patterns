using MassTransit;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.StateMachine
{
    public class OrderStateData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public required string CurrentState { get; set; }
        public Guid OrderId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }

        public bool IsBilled { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime? CanceledAt { get; set; }

        public Guid? PaymentTimeoutTokenId { get; set; }
    }
}
