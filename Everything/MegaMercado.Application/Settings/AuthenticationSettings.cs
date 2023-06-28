using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MegaMercado.Application.Settings;

public class AuthenticationSettings
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int Expires { get; set; } = 30;
    
    public byte[] GenerateSecretByte() => Encoding.ASCII.GetBytes(SecretKey);
}