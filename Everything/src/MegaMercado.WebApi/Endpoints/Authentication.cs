using MegaMercado.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MegaMercado.WebApi.Endpoints;

public static class Authentication
{
    public static WebApplication AddAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/login", ([FromBody] TokenService.CreateTokenInfo  user, TokenService tokenService) =>
        {
            var token = tokenService.GenerateToken(user);
            return Results.Ok(token);
        }).AllowAnonymous();

        return app;
    }
    
}