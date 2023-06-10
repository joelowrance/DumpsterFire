using Ardalis.Specification;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Domain.Specification;

public class CategoryByIdSpec : Specification<Category>
{
    public CategoryByIdSpec(int id)
    {
        Query
            //.Include(x => x.ProductCategories.OrderBy(x => x.Name))
            .Include(x => x.SubCategories)
            .Where(x => x.Id == id);
    }
}