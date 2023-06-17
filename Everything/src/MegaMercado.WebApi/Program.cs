using System.Security.Claims;
using MegaMercado.Application;
using MegaMercado.Infrastructure;
using MegaMercado.WebApi.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger(); //Log any start up problems

Log.Information("Start up");
IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty).Equals("development",
        StringComparison.InvariantCultureIgnoreCase)) 
{
    Log.Information("Using appsettings.Development.json");
    builder.Configuration.AddJsonFile("appsettings.Development.json");
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, _, cfg) =>
{
    cfg
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

builder.Services.AddControllers();
builder.Services.AddServicesFromInfrastructureLayer(builder.Configuration)
    .AddServicesFromApplicationLayer();

builder.Services.AddAuthentication(cfg =>
{
    //cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    //cfg.RequireHttpsMetadata = false;
    //cfg.SaveToken = true;
    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience =true,
        //ValidateLifetime = true,
        //ValidateIssuerSigningKey = true,
        //change these
        ValidIssuer = "JWTAuthenticationServer",
        ValidAudience = "JWTServicePostmanClient",
        IssuerSigningKey = new SymmetricSecurityKey(ApiSettings.GenerateSecretByte())
    };
    
    //cfg.Validate();
});


// builder.Services.AddAuthorization(cfg =>
// {
//     cfg.FallbackPolicy = cfg.DefaultPolicy;
//     cfg.AddPolicy("Admin", policy => policy.RequireClaim("role", "admin"));
//     cfg.AddPolicy("User", policy => policy.RequireClaim("role", "user"));
// });
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

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
app.AddProductEndpoints()
    .AddCategoryEndPoints()
    .AddShoppingCartEndPoints()
    .AddAuthenticationEndpoints();

app.MapPost("/wtf", (HttpContext context) =>
{
    return context.User.Claims.Select(c => new { c.Type, c.Value });
}).RequireAuthorization();

app.MapControllers();   
app.Run();


