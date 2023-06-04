namespace MegaMercado.Domain.Entities;

public class BlobbyHill
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string BlobType { get; set; } = string.Empty;
    public int BatchId { get; set; }
    public byte[] BlobData { get; set; } = null!;
}