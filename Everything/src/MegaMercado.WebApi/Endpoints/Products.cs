using MediatR;
using MegaMercado.Application.Products;

namespace MegaMercado.WebApi.Endpoints;

public static class Products
{
    public static WebApplication AddProductEndpoints(this WebApplication app)
    {
        app.MapGet("/product/{id:int}", async (IMediator mediator, int id) =>
        {
            var p = await mediator.Send(new GetProductByIdQuery(id));
            return p;
        });
        
        app.MapPut("/product/{id:int}", async (IMediator mediator, UpdateProductCommand command) => await mediator.Send(command))
            .RequireAuthorization("Admin");

        app.MapDelete("/product/{id:int}", async (IMediator mediator, int Id) => await mediator.Send( new DeleteProductCommand(Id)))
            .RequireAuthorization("Admin");

        app.MapPost("/product", async (IMediator mediator, CreateProductCommand command) => await mediator.Send(command))
            .RequireAuthorization("Admin");

        return app;
    }
}