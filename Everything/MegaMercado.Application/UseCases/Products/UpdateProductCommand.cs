using Ardalis.Specification;
using FluentValidation;
using MediatR;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.UseCases.Products;

public record UpdateProductCommand(int Id, string? Name, string? Description, decimal? Price, decimal? Rating, decimal? Msrp) : IRequest<bool>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IRepositoryBase<Product> productRepository, IRepositoryBase<Category> categoryRepository, IRepositoryBase<Brand> brandRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name));
        
        RuleFor(x => x.Description)
            .MinimumLength(25)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description));
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price is not null);
        
        RuleFor(x => x.Msrp)
            .GreaterThan(0)
            .When(x => x.Msrp is not null);


        RuleFor(x => x.Rating)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5)
            .When(x => x.Rating is not null);
    }
}

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

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.Price = request.Price ?? product.Price;
        product.Msrp = request.Msrp ?? product.Msrp;
        product.Rating = request.Rating ?? product.Rating;
        
        await _productRepository.UpdateAsync(product, cancellationToken);
        
        return true;
    }
}
