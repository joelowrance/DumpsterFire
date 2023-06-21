namespace MegaMercado.Application.Products.Dto;

public class CategoryOverviewModel
{
    public string Description { get; set; } = string.Empty;
    public int Id { get; set; }
}

public class  CategoryDetailsModel
{
    public string Description { get; set; } = string.Empty;
    public int Id { get; set; }
    public IEnumerable<CategoryOverviewModel> ChildCategories { get; set; } = Array.Empty<CategoryOverviewModel>();
    public Page<ProductDetailsModel> Products { get; set; } = null!;
}


public record Page<T>(IEnumerable<T> Items, int CurrentPage, int TotalCount, int ItemsPerPage);
