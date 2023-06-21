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

// public class ProductSearchSpecification : Specification<Product>
// {
//     protected ProductSearchSpecification(string search)
//     {
//         if (!string.IsNullOrWhiteSpace(search))
//         {
//             Query.Where(x => x.Name.Contains(search) || x.Description.Contains(search));
//         }
//
//         Query.Include(x => x.ProductCategories)
//             .ThenInclude(x => x.Category)
//             .Include(x => x.Brand);
//     }
// }