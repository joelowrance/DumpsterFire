using Ardalis.Specification;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Domain.Specification;

public class ProductByIdSpec : Specification<Product>
{
    public ProductByIdSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Categories)
            .Include(x => x.Brand);
    }
}
