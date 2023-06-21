using Ardalis.Specification;
using MediatR;
using MegaMercado.Application.Products;
using MegaMercado.Application.Products.Dto;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.UseCases.Products;

public record GetProductByIdQuery(int ProductId) : IRequest<ProductDetailsModel?>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsModel?>
{
    private readonly IRepositoryBase<Product> _productRepository;

    public GetProductByIdQueryHandler(IRepositoryBase<Product> productRepository)
    {
        _productRepository = productRepository;
    }


    public async Task<ProductDetailsModel?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductByIdSpec(request.ProductId);
        var products = await _productRepository.FirstOrDefaultAsync(spec, cancellationToken);
        return products.ToProductDetailsModel();
    }
}