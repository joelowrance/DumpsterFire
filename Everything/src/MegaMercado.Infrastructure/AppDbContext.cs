using System.Reflection;
using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    //public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<BlobbyHill> Blobs => Set<BlobbyHill>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}

//EF cli complained about this
public class AppDbContextInitializer
{
    
}