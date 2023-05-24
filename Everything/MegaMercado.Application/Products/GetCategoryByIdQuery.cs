using Ardalis.Specification;
using MediatR;
using MegaMercado.Application.Products.Dto;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.Products;

public record GetCategoryByIdQuery(int CategoryId, int Page) : IRequest<CategoryDetailsModel>
{
    public GetCategoryByIdQuery(int categoryId, int? page) : this(categoryId, page ?? 1)
    {
    }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDetailsModel?>
{
    private readonly IRepositoryBase<Category> _categoryRepository;

    public GetCategoryByIdQueryHandler(IRepositoryBase<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDetailsModel?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new CategoryByIdSpec(request.CategoryId);
        var category = await _categoryRepository.FirstOrDefaultAsync(spec, cancellationToken);
        return category?.ToCategoryDetailsModel();
    }
}
