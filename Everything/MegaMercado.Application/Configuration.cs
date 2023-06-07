using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MegaMercado.Application;

public static class Configuration
{
    public static IServiceCollection AddServicesFromApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}