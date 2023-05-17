using System.Reflection;
using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}

public class AppDbContextInitializer
{
    
}