using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "JWTAuthenticationServer",
            ValidAudience = "JWTServicePostmanClient",
            //ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("your_secret_key_here")),
        };
    });


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/token", () =>
{
    var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, "Subject"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim("UserId", 1.ToString()),
        new Claim("DisplayName", "joe"),
        new Claim("UserName", "joe joe"),
        new Claim("Email", "joe@joe.com")
    };
    
    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("your_secret_key_here"));
    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Issuer = "JWTAuthenticationServer",
        Audience = "JWTServicePostmanClient",
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
});

app.MapGet("/anon", async (ClaimsPrincipal user) =>
{
    return user.Claims.ToList();
}).AllowAnonymous();

app.MapGet("/private", async (ClaimsPrincipal user) =>
{
    return user.Claims.Select(x => new { x.Type, x.Value }).ToList();
}).RequireAuthorization();


app.Run();