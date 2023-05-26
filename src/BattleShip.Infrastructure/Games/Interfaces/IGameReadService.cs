namespace BattleShip.Infrastructure.Games.Interfaces;

using BattleShip.Application.Games.ViewModel;

internal interface IGameReadService
{
    Task<Game> GetGameAsync(CancellationToken cancellationToken = default);
}
