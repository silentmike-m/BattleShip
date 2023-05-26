namespace BattleShip.Infrastructure.Games.Interfaces;

using BattleShip.Application.Games.ViewModel;

internal interface IGameReadService
{
    Task<string> GetCellStatusAsync(int column, char row, CancellationToken cancellationToken = default);
    Task<Game> GetGameAsync(CancellationToken cancellationToken = default);
}
