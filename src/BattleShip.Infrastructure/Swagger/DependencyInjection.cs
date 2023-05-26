namespace BattleShip.Infrastructure.Swagger;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

internal static class DependencyInjection
{
    public static IServiceCollection AddBattleshipSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSwaggerGen(c =>
        {
            c.CustomSchemaIds(s => s.FullName);
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Battleship",
                Version = "v1",
            });
        });

        return services;
    }

    public static void UseBattleshipSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "Battleship v1");
            options.OAuthClientId("swagger");
            options.OAuthUsePkce();
        });
    }
}
