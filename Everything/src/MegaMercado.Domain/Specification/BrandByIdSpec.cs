using Ardalis.Specification;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Domain.Specification;

public class BrandByIdSpec : Specification<Brand>
{
    public BrandByIdSpec(int id)
    {
        Query
            .Where(x => x.Id == id);
    }
}