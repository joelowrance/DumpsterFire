using System.Reflection;
using FluentValidation;
using MediatR;
using MegaMercado.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MegaMercado.Application;

public static class Configuration
{
    public static IServiceCollection AddServicesFromApplicationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        

        return services;
    }
}