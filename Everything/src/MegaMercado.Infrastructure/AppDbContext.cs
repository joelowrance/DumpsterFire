using System.Reflection;
using MegaMercado.Application.Common;
using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    //private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTime;

    public AuditableEntitySaveChangesInterceptor(
      // ICurrentUserService currentUserService,
        IDateTimeProvider dateTime)
    {
        //_currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseChangeTrackEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                //entry.Entity.CreatedBy = _currentUserService.UserId;
                entry.Entity.Created = _dateTime.NowOffset;
            } 

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                //entry.Entity.LastModified = _currentUserService.UserId;
                entry.Entity.Modified = _dateTime.NowOffset;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.Modified = _dateTime.NowOffset;
                entry.Entity.Deleted  = _dateTime.NowOffset;
                entry.Entity.IsDeleted = true;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
