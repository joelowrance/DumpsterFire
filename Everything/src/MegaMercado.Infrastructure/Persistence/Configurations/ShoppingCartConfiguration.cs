using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaMercado.Infrastructure.Persistence.Configurations;

// public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
// {
//     public void Configure(EntityTypeBuilder<ShoppingCart> builder)
//     {
//         builder.Property(x => x.CustomerId).IsRequired();
//     }
// }