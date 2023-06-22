using MediatR;
using MegaMercado.Application.Common;
using MegaMercado.Application.Products.Dto;
using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Application.UseCases.Products;

public record ProductSearchQuery
    (string Query, int Page, int RecordsPerPage) : IRequest<PagedList<ProductDetailsModel>>
{
}

public class ProductsSearchQueryHandler : IRequestHandler<ProductSearchQuery, PagedList<ProductDetailsModel>>
{
    private IAppDbContext _appDbContext;

    public ProductsSearchQueryHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<PagedList<ProductDetailsModel>> Handle(ProductSearchQuery request, CancellationToken cancellationToken)
    {

        var query = _appDbContext.Products.AsQueryable();
             
             
        if (!string.IsNullOrEmpty(request.Query))
        {
            query = query.Where(x => x.Description.Contains(request.Query) || x.Name.Contains(request.Query));
        }
        
        query = query.Include(x => x.ProductCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.Brand);
        
        var mapped = query.Select(product =>
            new ProductDetailsModel
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
            });
        
        
        return await PagedList<ProductDetailsModel>.CreateAsync(mapped, request.Page, request.RecordsPerPage);
    }
}
