using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MegaMercado.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MegaMercado.WebApi.Services;

public class TokenService
{
    private readonly IOptions<AuthenticationSettings> _authenticationSettings;

    public TokenService(IOptions<AuthenticationSettings> authenticationSettings)
    {
        _authenticationSettings = authenticationSettings;
    }

    public string GenerateToken(CreateTokenInfo createTokenInfo)
    {
        //var userName = ActiveDirectoryHelper.GetUsernameFromEmail(loginInfo.EmailAddress);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _authenticationSettings.Value.GenerateSecretByte();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _authenticationSettings.Value.Issuer,
            Audience = _authenticationSettings.Value.Audience,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Subject =  new ClaimsIdentity()
        };
        
        
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Name, createTokenInfo.Name ?? string.Empty));
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, createTokenInfo.EmailAddress ?? string.Empty));
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "User"));

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    public class CreateTokenInfo
    {
        public int Id { get; set; }
        public string? EmailAddress { get; set; } 
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}