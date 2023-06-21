using MegaMercado.Application.Common;
using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MegaMercado.Infrastructure;

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
                entry.Entity.Created = _dateTime.OffsetNow;
            } 

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                //entry.Entity.LastModified = _currentUserService.UserId;
                entry.Entity.Modified = _dateTime.OffsetNow;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.Modified = _dateTime.OffsetNow;
                entry.Entity.Deleted  = _dateTime.OffsetNow;
                entry.Entity.IsDeleted = true;
                entry.State = EntityState.Modified;

                // Detach all references to deleted entities, otherwise they will be deleted too.
                foreach (var entryReference in entry.Navigations)
                {
                    entryReference.IsModified = false;
                    if (entryReference.EntityEntry.State == EntityState.Deleted)
                    {
                        entryReference.EntityEntry.State = EntityState.Modified;
                    }
                    //entryReference?.TargetEntry?.State = EntityState.Detached;
                }
            }
        }
    }
}