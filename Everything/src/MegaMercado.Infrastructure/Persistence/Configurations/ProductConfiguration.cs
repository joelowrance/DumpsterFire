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
        
        builder.HasOne<Brand>(x => x.Brand)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.BrandId);

        builder
            .HasMany<ProductCategory>(x => x.ProductCategories)
            .WithOne(x => x.Product)
            .OnDelete(DeleteBehavior.Cascade);
    }
}