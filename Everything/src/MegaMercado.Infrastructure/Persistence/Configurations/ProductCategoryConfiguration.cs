using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaMercado.Infrastructure.Persistence.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(x => new {x.ProductId, x.CategoryId});
        
        builder
            .HasOne<Product>(x => x.Product)
            .WithMany(x => x.ProductCategories)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne<Category>(x => x.Category)
            .WithMany(x => x.ProductCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Id)
            .HasColumnName("EF_NOT_USED");
    }
}