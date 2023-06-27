using System.Security.Claims;
using MegaMercado.Application;
using MegaMercado.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace MegaMercado.WebApi.Endpoints;

public static class Authentication
{
    public static WebApplication AddAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/login", ([FromBody] User  user, TokenService tokenService) =>
        {
            var token = tokenService.GenerateToken(user);
            return Results.Ok(token);
        }).AllowAnonymous();

        return app;
    }
    
}


public class CurrentUserService: IUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        Claims = httpContext?.User.Claims.ToList() ?? new List<Claim>();
    }

    public List<Claim> Claims { get; init; }
}
