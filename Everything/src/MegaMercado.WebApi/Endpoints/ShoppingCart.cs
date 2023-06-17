using MediatR;
using MegaMercado.Application.ShoppingCart;

namespace MegaMercado.WebApi.Endpoints;

public static class ShoppingCart
{
    public static WebApplication AddShoppingCartEndPoints(this WebApplication app)
    {
        app.MapPost("/cart", async (IMediator mediator, AddItemToCartCommand command) => await mediator.Send(command))
            .RequireAuthorization("User");


        return app;
    }
    
}