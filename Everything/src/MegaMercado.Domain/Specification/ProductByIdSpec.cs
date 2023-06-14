using Ardalis.Specification;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Domain.Specification;

public class ProductByIdSpec : Specification<Product>
{
    public ProductByIdSpec(int id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.ProductCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.Brand);
    }
}


public class ProductNameExistsSpec : Specification<Product>
{
    public ProductNameExistsSpec(string name)
    {
        Query.Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .AsNoTracking();
    }
}