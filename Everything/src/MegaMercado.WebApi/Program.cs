using MegaMercado.Domain.Entities;
using MegaMercado.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger(); //Log any start up problems

Log.Information("Start up");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty).Equals("development",
        StringComparison.InvariantCultureIgnoreCase)) ;
{
    Log.Information("Using appsettings.Development.json");
    builder.Configuration.AddJsonFile("appsettings.Development.json");
}

builder.Host.UseSerilog((context, services, cfg) =>
{
    cfg
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Product).Assembly));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MegaMercado"),
        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

//builder.Services.AddDbContext<>()/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();