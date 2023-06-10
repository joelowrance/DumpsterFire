namespace MegaMercado.Domain.Entities;

public class Category : BaseChangeTrackEntity
{
    public string Description { get; set; } = string.Empty;
    public List<Category> SubCategories { get; set; } = new();
    public List<ProductCategory> ProductCategories { get; set; } = new();
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
}