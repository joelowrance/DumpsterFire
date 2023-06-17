using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MegaMercado.Application;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}

public static class ApiSettings
{
    public static string SecretKey = "fedaf7d8863b48e197b9287d492b708e";
    public static byte[] GenerateSecretByte() => 
        Encoding.ASCII.GetBytes(SecretKey);
} 

public class TokenService
{
    public string GenerateToken(User user)
    {
        //var userName = ActiveDirectoryHelper.GetUsernameFromEmail(loginInfo.EmailAddress);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = ApiSettings.GenerateSecretByte();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "JWTAuthenticationServer",
            Audience = "JWTServicePostmanClient",
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            
            
        };
        
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Name, "joejoe"));
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Email, "joejoe@joe.com"));
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "admin"));
        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "user"));

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public void foo()
    {
        
        // var tokenHandler = new JwtSecurityTokenHandler();
        // var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        // var tokenDescriptor = new SecurityTokenDescriptor
        // {
        //     Subject = new ClaimsIdentity(new List<Claim> 
        //     {
        //         new(ClaimTypes.Name, userName),
        //         new(ClaimTypes.Email, loginInfo.EmailAddress),
        //     }),
        //         
        //     Expires = DateTime.Now.AddDays(90),
        //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
        // };
            
        //        var roles = _roleProvider.GetUserRoles(loginInfo.EmailAddress);
            
            
        // If no roles, treat as unauthorized
        // if (!roles.Any())
        // {
        //     return null;
        // }
        //     
        // foreach (var role in roles)
        // {
        //     tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
        // }
        //     
        // var token = tokenHandler.CreateToken(tokenDescriptor);
        // return new Tokens { Token = tokenHandler.WriteToken(token) };

    }
}