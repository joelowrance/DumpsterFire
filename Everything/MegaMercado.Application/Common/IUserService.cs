using System.Security.Claims;

namespace MegaMercado.Application.Common;

public interface IUserService
{
    public List<Claim> Claims { get; }

    public string Email { get; }
}