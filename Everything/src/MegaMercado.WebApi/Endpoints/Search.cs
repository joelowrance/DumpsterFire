using MediatR;
using MegaMercado.Application.UseCases.Search;
using Microsoft.AspNetCore.Mvc;

namespace MegaMercado.WebApi.Endpoints;

public static class Search
{
    public static WebApplication AddSearchEndPoints(this WebApplication app)
    {
        app.MapPost("/search/BuildIndex", async (IMediator mediator) =>
        {
            await mediator.Publish(new BuildApplicationIndexNotification());
            return Results.Ok("creating index");
        }).AllowAnonymous();
        
        app.MapGet("/search", async (IMediator mediator, [FromQuery] string query) 
            => await mediator.Send(new SearchQuery(query))).AllowAnonymous();

        return app;
    }
}