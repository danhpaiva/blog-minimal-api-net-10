using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Post");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(p => p.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(160);

        builder.Property(p => p.Summary)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);

        builder.Property(p => p.Body)
            .IsRequired()
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);

        builder.HasIndex(p => p.Slug, "IX_Post_Slug")
            .IsUnique();

        builder.Property(p => p.CreateDate)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.LastUpdateDate)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValue(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));;

        builder.HasIndex(p => p.CreateDate, "IX_Post_CreateDate").IsUnique();
        builder.HasIndex(p => p.LastUpdateDate, "IX_Post_LastUpdateDate").IsUnique();
        builder.HasIndex(p => p.Title, "IX_Post_Title").IsUnique();
        builder.HasIndex(p => p.Slug, "IX_Post_Slug").IsUnique();

        // Relationship Post - Author (N:1)
        builder
            .HasOne(p => p.Author)
            .WithMany(c => c.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship Post - Category (N:1)
        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.Tags)
            .WithMany(t => t.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                post => post
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_PostTag_TagId")
                    .OnDelete(DeleteBehavior.Cascade),
                tag => tag
                    .HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostTag_PostId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
