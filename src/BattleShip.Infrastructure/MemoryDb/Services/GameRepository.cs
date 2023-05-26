namespace BattleShip.Infrastructure.MemoryDb.Services;

using BattleShip.Application.Commons.Extensions;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Repositories;
using BattleShip.Infrastructure.MemoryDb.Converters;
using BattleShip.Infrastructure.MemoryDb.Interfaces;

internal sealed class GameRepository : IGameRepository
{
    private readonly IDbContext context;

    public GameRepository(IDbContext context)
        => this.context = context;

    public GameEntity? Get()
    {
        if (string.IsNullOrWhiteSpace(this.context.Game))
        {
            return null;
        }

        return this.context.Game.To<GameEntity>(new JsonStringEnumConverter(), new ShipConverter());
    }

    public void Save(GameEntity game)
    {
        this.context.Game = game.ToJson(new JsonStringEnumConverter(), new ShipConverter());
    }
}
