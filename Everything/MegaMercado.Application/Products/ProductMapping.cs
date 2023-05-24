using MegaMercado.Application.Products.Dto;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Application.Products;

public static class ProductMapping
{
    public static ProductDetailsModel? ToProductDetailsModel(this Product? product)
    {
        if (product is null) return null;

        var model = new ProductDetailsModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            BrandName = product.Brand.Name,
            BrandId = product.Brand.Id,
            Price = product.Price,
            Msrp = product.Msrp,
            Type = product.Type,
            Rating = product.Rating,
        };
        
        model.Categories.Add(product.Categories[0].ToCategoryOverviewModel());
        model.Categories.Add(product.Categories[1].ToCategoryOverviewModel());

        return model;
    }
    
    private static ProductDetailsModel ToProductDetailsModelSafe(this Product product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        var model = new ProductDetailsModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            BrandId = product.BrandId,
            Price = product.Price,
            Msrp = product.Msrp,
            Type = product.Type,
            Rating = product.Rating,
            Categories = (product.Categories)
                .Select(x => x.ToCategoryOverviewModel())
                .ToList()
        };

        return model;
    }

    private static CategoryOverviewModel ToCategoryOverviewModel(this Category category)
    {
        return new CategoryOverviewModel { Id = category.Id, Description = category.Description };
    }

    public static CategoryDetailsModel ToCategoryDetailsModel(this Category category)
    {
        return new CategoryDetailsModel
        {
            Id = category.Id,
            Description = category.Description,
            ChildCategories = category.SubCategories.Select(x => x.ToCategoryOverviewModel()),
            Products = new Page<ProductDetailsModel>(
                category.Products.Take(100).Select(x => x.ToProductDetailsModelSafe()), 1, category.Products.Count, 100)
        };
    } 
}