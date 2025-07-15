using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.DbExtension
{
    public class OrderStateMap : IEntityTypeConfiguration<OrderStateData>
    {
        public void Configure(EntityTypeBuilder<OrderStateData> builder)
        {
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.CurrentState).HasMaxLength(64);
            builder.Property(x => x.OrderId);
            builder.Property(x => x.Amount);
            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.PaidAt);
            builder.Property(x => x.CanceledAt);

            // Optionally configure other aspects like indexes
            builder.HasIndex(x => x.OrderId);
        }
    }
}
