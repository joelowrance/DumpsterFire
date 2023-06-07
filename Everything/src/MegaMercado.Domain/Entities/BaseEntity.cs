namespace MegaMercado.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
}

public abstract class BaseChangeTrackEntity : BaseEntity
{
    public DateTimeOffset Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTimeOffset? Deleted { get; set; }
}

