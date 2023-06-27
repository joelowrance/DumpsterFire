using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MegaMercado.DatasetImport.EF;

public class LearningContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Brand> Brands { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=127.0.0.1;Database=MegaMercadoImport;User Id=sa;Password=$1Password99();TrustServerCertificate=Yes;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningContext).Assembly);
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasMany<Category>(x => x.Categories)
            .WithMany(x => x.Products);

        builder.Property(x => x.Msrp)
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.Rating)
            .HasColumnType("decimal(18,2)");

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

        // builder.HasOne<Category>(x => x.ParentCategory)
        //     .WithMany(x => x.SubCategories)
        //     .HasForeignKey(x => x.ParentCategoryId)
        //     .OnDelete(DeleteBehavior.Restrict);


        // builder.HasMany<Category>(x => x.SubCategories)
        //     .WithOne(x => x.ParentCategory)
        //     .OnDelete(DeleteBehavior.Restrict);

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