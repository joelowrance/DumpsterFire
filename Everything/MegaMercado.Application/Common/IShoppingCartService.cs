using MegaMercado.Domain.ShoppingCart;

namespace MegaMercado.Application.Common;

public interface IShoppingCartService
{
    Cart GetCart(Guid userId);
    Cart AddItemToCart(Guid userId, LineItem lineItem);
    Cart RemoveItemFromCart(Guid userId, int productid);
}