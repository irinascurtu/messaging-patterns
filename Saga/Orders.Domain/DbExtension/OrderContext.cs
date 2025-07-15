using Microsoft.EntityFrameworkCore;
using Orders.Domain.DbExtension;
using Orders.Domain.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain
{
    public partial class OrderContext : DbContext
    { 
        public DbSet<OrderStateData> OrderStates { get; set; }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderStateMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
