using System.Security.Claims;
using MegaMercado.Domain.ShoppingCart;

namespace MegaMercado.Application.Common;

public interface IShoppingCartService
{
    Cart GetCart(string emailAddress);
    Cart AddItemToCart(string emailAddress, LineItem lineItem);
    Cart RemoveItemFromCart(string emailAddress, int productid);
}

public interface IUserService
{
    public List<Claim> Claims { get; }
    
    public string Email => Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
}

