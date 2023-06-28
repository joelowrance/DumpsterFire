using System.Security.Claims;
using MegaMercado.Application.Common;

namespace MegaMercado.WebApi.Services;

public class CurrentUserService: IUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        Claims = httpContext?.User.Claims.ToList() ?? new List<Claim>();
    }

    public List<Claim> Claims { get; init; }
    public string Email => Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
}