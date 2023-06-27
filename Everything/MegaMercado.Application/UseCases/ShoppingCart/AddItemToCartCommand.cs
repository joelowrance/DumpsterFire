using Ardalis.Specification;
using MediatR;
using MegaMercado.Application.Common;
using MegaMercado.Domain.Entities;
using MegaMercado.Domain.ShoppingCart;
using MegaMercado.Domain.Specification;

namespace MegaMercado.Application.UseCases.ShoppingCart;

public record AddItemToCartCommand(int ProductId) : IRequest<Cart>;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Cart>
{
    private readonly IRepositoryBase<Product> _productRepository;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IUserService _userService;

    public AddItemToCartCommandHandler(
        IRepositoryBase<Product> productRepository, 
        IShoppingCartService shoppingCartService, 
        IUserService userService)
    {
        _productRepository = productRepository;
        _shoppingCartService = shoppingCartService;
        _userService = userService;
    }

    public async Task<Cart> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var p = await _productRepository.FirstOrDefaultAsync(new ProductByIdSpec(request.ProductId), cancellationToken);

        if (p is null)
        {
            throw new NullReferenceException($"Product not found: { request.ProductId }");
        }
        
        var cart = _shoppingCartService.AddItemToCart(_userService.Email, new LineItem(p.Id, p.Msrp, p.Price)  { Quantity = 1});
        return cart;
    }
}
