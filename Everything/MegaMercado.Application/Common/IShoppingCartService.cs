using MegaMercado.Domain.ShoppingCart;

namespace MegaMercado.Application.Common;

public interface IShoppingCartService
{
    Cart GetCart(string emailAddress);
    Cart AddItemToCart(string emailAddress, LineItem lineItem);
    Cart RemoveItemFromCart(string emailAddress, int productid);
}