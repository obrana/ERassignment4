using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFAssinment4
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    

        public class CategoryConfiguration : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {
                builder.ToTable("categories");
                builder.Property(x => x.Id).HasColumnName("categoryid");
                builder.Property(x => x.Name).HasColumnName("categoryname");
                builder.Property(x => x.Description).HasColumnName("description");
            }
        }
    }
}