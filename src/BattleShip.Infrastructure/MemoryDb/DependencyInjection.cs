namespace BattleShip.Infrastructure.MemoryDb;

using BattleShip.Domain.Repositories;
using BattleShip.Infrastructure.Games.Interfaces;
using BattleShip.Infrastructure.MemoryDb.Interfaces;
using BattleShip.Infrastructure.MemoryDb.Services;
using Microsoft.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddMemoryDb(this IServiceCollection services)
    {
        services.AddSingleton<IDbContext, DbContext>();

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGameReadService, GameReadService>();

        return services;
    }
}
