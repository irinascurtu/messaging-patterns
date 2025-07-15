using Microsoft.EntityFrameworkCore;
using ProductsApi.Data.Entities;
using ProductsApi.Models;
using System.Collections.Generic;

namespace ProductsApi.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
