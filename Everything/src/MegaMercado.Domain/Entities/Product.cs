namespace MegaMercado.Domain.Entities;

public class Product : BaseChangeTrackEntity
{
    public string Name { get; set; } = string.Empty;
    
    public List<ProductCategory> ProductCategories { get; set; } = new();

    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public decimal Price { get; set; }
    public decimal Msrp { get; set; }
    public string Type { get; set; }
    public decimal Rating { get; set; }
    public string Description { get; set; }
}

public class ProductCategory : BaseChangeTrackEntity
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public Product Product { get; set; } = null!;
    public Category Category { get; set; } = null!;
}