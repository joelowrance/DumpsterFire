namespace MegaMercado.Domain.Entities;

public class Brand : BaseChangeTrackEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}