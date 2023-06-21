using MediatR;
using MegaMercado.Application.Products;
using MegaMercado.Application.UseCases.Products;

namespace MegaMercado.WebApi.Endpoints;

public static class Categories
{
    public static WebApplication AddCategoryEndPoints(this WebApplication app)
    {
        app.MapGet("/category/{id:int}", async (IMediator mediator, int id, int? page) =>
        {
            var p = await mediator.Send(new GetCategoryByIdQuery(id, page));
            return p;
        });

        return app;
    }
}