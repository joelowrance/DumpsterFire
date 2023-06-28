using Ardalis.Specification;
using MegaMercado.Application.Common;
using MegaMercado.Infrastructure.Persistence.Redis;
using MegaMercado.Infrastructure.Repositories;
using MegaMercado.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MegaMercado.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddServicesFromInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MegaMercado"),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        });

        services.AddSingleton<RedisConnection>(x => new RedisConnection(configuration.GetConnectionString("redis")!));
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped(typeof(IRepositoryBase<>), typeof(SpecRepository<>));
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
        services.AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }
}