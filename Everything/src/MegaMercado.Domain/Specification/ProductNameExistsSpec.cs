using Ardalis.Specification;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Domain.Specification;

public class ProductNameExistsSpec : Specification<Product>
{
    public ProductNameExistsSpec(string name)
    {
        Query.Where(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .AsNoTracking();
    }
}