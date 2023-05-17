using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaMercado.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Msrp)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.Rating)
            .HasColumnType("decimal(18,2)");

        builder.HasMany(x => x.Categories)
            .WithMany(x => x.Products);
        
        builder.HasOne<Brand>(x => x.Brand)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.BrandId);
    }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasOne<Category>(x => x.ParentCategory)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentCategoryId)
            .IsRequired(false);

        builder.HasMany<Product>(x => x.Products)
            .WithMany(x => x.Categories);
    }
}

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
    }
}