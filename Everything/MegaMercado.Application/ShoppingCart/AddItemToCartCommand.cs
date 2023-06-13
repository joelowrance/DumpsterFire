using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MediatR;
using MegaMercado.Application.Common;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.ShoppingCart;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.ShoppingCart;

public record AddItemToCartCommand(int ProductId) : IRequest<Cart>;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Cart>
{
    private readonly IRepositoryBase<Product> _productRepository;
    private readonly IShoppingCartService _shoppingCartService;

    public AddItemToCartCommandHandler(IRepositoryBase<Product> productRepository, IShoppingCartService shoppingCartService)
    {
        _productRepository = productRepository;
        _shoppingCartService = shoppingCartService;
    }

    public async Task<Cart> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var p = await _productRepository.FirstOrDefaultAsync(new ProductByIdSpec(request.ProductId), cancellationToken);

        if (p is null)
        {
            throw new NullReferenceException($"Product not found: { request.ProductId }");
        }
        
        var cart = _shoppingCartService.AddItemToCart(new Guid("F456BA68-D279-45F8-A54C-F0C9DC315B47"), new LineItem(p.Id, p.Msrp, p.Price)  { Quantity = 1});
        return cart;
    }
}

