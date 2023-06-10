using Ardalis.Specification;
using FluentValidation;
using MediatR;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.Products;

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


public record UpdateProductCommand(int Id, string Name, string Description, decimal Price, decimal Rating) : IRequest<bool>;

public record UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IRepositoryBase<Product> _productRepository;

    public UpdateProductCommandHandler(IRepositoryBase<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product  =await _productRepository.FirstOrDefaultAsync(new ProductByIdSpec(request.Id), cancellationToken);

        if (product == null) return false;
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Rating = request.Rating;
        
        await _productRepository.UpdateAsync(product, cancellationToken);
        
        return true;
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100);
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(25)
            .MaximumLength(500);
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);


        RuleFor(x => x.Rating)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5);
    }
}