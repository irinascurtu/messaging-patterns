using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;

namespace OrdersApiNSB.Domain
{
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//
            base.OnModelCreating(modelBuilder);

        }
    }
}
