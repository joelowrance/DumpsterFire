using System.Net.Mime;
using System.Text;
using MediatR;
using MegaMercado.Application;
using MegaMercado.Application.Products;
using MegaMercado.Application.ShoppingCart;
using MegaMercado.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger(); //Log any start up problems

Log.Information("Start up");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty).Equals("development",
        StringComparison.InvariantCultureIgnoreCase)) 
{
    Log.Information("Using appsettings.Development.json");
    builder.Configuration.AddJsonFile("appsettings.Development.json");
}

builder.Host.UseSerilog((context, _, cfg) =>
{
    cfg
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

builder.Services.AddServicesFromInfrastructureLayer(builder.Configuration)
    .AddServicesFromApplicationLayer();


builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(ApiSettings.GenerateSecretByte()),
        ValidateIssuer = false,
        ValidateAudience = false,
    };

});

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("Admin", policy => policy.RequireClaim("role", "admin"));
    cfg.AddPolicy("User", policy => policy.RequireClaim("role", "user"));

    //create a policy to allow anonymous users
    cfg.AddPolicy("Anonymous", policy => policy.RequireAssertion(_ => true));
    cfg.DefaultPolicy = cfg.GetPolicy("Anonymous")!;
        
});
    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/product/{id:int}", async (IMediator mediator, int id) =>
{
    var p = await mediator.Send(new GetProductByIdQuery(id));
    return p;
});

app.MapGet("/category/{id:int}", async (IMediator mediator, int id, int? page) =>
{
    var p = await mediator.Send(new GetCategoryByIdQuery(id, page));
    return p;
});

app.MapPut("/product/{id:int}", async (IMediator mediator, UpdateProductCommand command) =>
{
    return await mediator.Send(command);
}).RequireAuthorization("Admin");

app.MapDelete("/product/{id:int}", async (IMediator mediator, int Id) =>
{
    
    return await mediator.Send( new DeleteProductCommand(Id));
}).RequireAuthorization("Admin");

// create a post endpoint to create a product
app.MapPost("/product", async (IMediator mediator, CreateProductCommand command) =>
{
    return await mediator.Send(command);
}).RequireAuthorization("Admin");

// create a post endpoint to create a product
app.MapPost("/cart", async (IMediator mediator, AddItemToCartCommand command) =>
{
    return await mediator.Send(command);
}).RequireAuthorization("User");

app.MapPost("/login", ([FromBody] User  user, TokenService tokenService) =>
{
    var token = tokenService.GenerateToken(user);
    return Results.Ok(token);
}).AllowAnonymous();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


