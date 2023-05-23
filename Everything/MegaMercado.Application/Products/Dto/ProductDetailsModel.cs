namespace MegaMercado.Application.Products.Dto;

public class ProductDetailsModel
{
    public string Name { get; set; } = string.Empty;
    public List<CategoryDetailsModel> Categories { get; set; } = new();

    public int BrandId { get; set; }
    
    public decimal Price { get; set; }
    public decimal Msrp { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public string Description { get; set; }= string.Empty;
    public int Id { get; set; }
    public string BrandName { get; set; }= string.Empty;
}