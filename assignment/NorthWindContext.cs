using assignment;
using EFAssinment4;
using Microsoft.EntityFrameworkCore;

namespace EFassinment4
{
    public class NorthWindContext : DbContext
    {
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost;db=northwind;uid=postgres;pwd=ktm145");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new Category.CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}