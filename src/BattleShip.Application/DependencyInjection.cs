namespace BattleShip.Application;

using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        return services;
    }
}
