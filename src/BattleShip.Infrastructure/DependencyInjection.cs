namespace BattleShip.Infrastructure;

using System.Reflection;
using BattleShip.Infrastructure.MemoryDb;
using BattleShip.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddMemoryDb();
        services.AddBattleshipSwagger(configuration);

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseBattleshipSwagger();
    }
}
