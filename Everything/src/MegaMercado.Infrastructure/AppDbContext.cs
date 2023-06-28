using System.Reflection;
using MegaMercado.Application.Common;
using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MegaMercado.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<BlobbyHill> Blobs => Set<BlobbyHill>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public AppDbContext(
        DbContextOptions options, 
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
    ) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}

//EF cli complained about this
public class AppDbContextInitializer
{
    
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
