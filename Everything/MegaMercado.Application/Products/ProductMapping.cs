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
        
        model.Categories.Add(product.Categories[0].ToCategoryDetailsModel());
        model.Categories.Add(product.Categories[1].ToCategoryDetailsModel());

        return model;
    }

    private static CategoryDetailsModel ToCategoryDetailsModel(this Category category)
    {
        return new CategoryDetailsModel { Id = category.Id, Description = category.Description };
    }
    
    
}