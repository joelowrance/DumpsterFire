using Ardalis.Specification;
using FluentValidation;
using MediatR;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Application.UseCases.Products;

public record CreateProductCommand(string Name, string Description, decimal Price, decimal Msrp, decimal Rating,
    int[] Categories, int BrandId, string Type) : IRequest<int>;
    
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IRepositoryBase<Product> productRepository, IRepositoryBase<Category> categoryRepository, IRepositoryBase<Brand> brandRepository)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Price).NotEmpty().GreaterThan(1);
        RuleFor(x => x.Msrp).NotEmpty().GreaterThan(1);
        RuleFor(x => x.Rating).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Categories).NotEmpty();
        RuleFor(x => x.Type).NotEmpty().MinimumLength(5);
        RuleFor(x => x.BrandId).GreaterThan(0);
    }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IRepositoryBase<Product> _productRepository;
    private readonly IRepositoryBase<Category> _categoryRepository;

    public CreateProductCommandHandler(IRepositoryBase<Product> productRepository, IRepositoryBase<Category> categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Rating = request.Rating,
            Type = request.Type,
            BrandId = request.BrandId,
            Msrp = request.Msrp,
            ProductCategories = request.Categories.Select(x => new ProductCategory { CategoryId = x }).ToList()
        };

        await _productRepository.AddAsync(product, cancellationToken);

        return product.Id;
    }
}

