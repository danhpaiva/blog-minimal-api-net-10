using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //Table name
        builder.ToTable("Category");

        //Primary Key
        builder.HasKey(c => c.Id);

        //Identity
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); // SQL Server Identity (1,1)

        //Properties
        builder.Property(c => c.Name)
            .IsRequired() // Not Null
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        //Indexes
        builder.HasIndex(c => c.Slug, "IX_Category_Slug")
            .IsUnique();
    }
}
