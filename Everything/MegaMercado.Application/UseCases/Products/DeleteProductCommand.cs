using Ardalis.Specification;
using MediatR;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.UseCases.Products;

public record DeleteProductCommand(int Id) : IRequest<bool>;

public record DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IRepositoryBase<Product> _productRepository;

    public DeleteProductCommandHandler(IRepositoryBase<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FirstOrDefaultAsync(new ProductByIdSpec(request.Id), cancellationToken);

        if (product == null) return false;

        await _productRepository.DeleteAsync(product, cancellationToken);
        
        return true;
    }
}
