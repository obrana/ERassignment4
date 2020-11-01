using Microsoft.EntityFrameworkCore;

namespace EFassinment4
{
    public class NorthWindContext : DbContext
    {
        public DbSet<Category>Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("host=localhost;db=northwind;uid=postgres;pwd=ktm145");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Category.CategoryConfiguration());
        }
    }
} 