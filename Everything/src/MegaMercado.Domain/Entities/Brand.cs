namespace MegaMercado.Domain.Entities;

public class Brand : BaseChangeTrackEntity
{
    public string Name { get; set; } = string.Empty;
    public List<Product> Products { get; set; } = new();
}