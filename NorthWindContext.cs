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
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Category>().Property(x=> x.Id).HasColumnName("categoryid");
            modelBuilder.Entity<Category>().Property(x=> x.Name).HasColumnName("categoryname");
            modelBuilder.Entity<Category>().Property(x=> x.Description).HasColumnName("description");
        }
    }
} 