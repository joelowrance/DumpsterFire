using MediatR;
using MegaMercado.Application;
using MegaMercado.Application.Products;
using MegaMercado.Infrastructure;
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();