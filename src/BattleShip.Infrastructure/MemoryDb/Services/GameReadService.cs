namespace BattleShip.Infrastructure.MemoryDb.Services;

using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.ViewModel;
using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Repositories;
using BattleShip.Infrastructure.Games.Interfaces;

internal sealed class GameReadService : IGameReadService
{
    private readonly IGameRepository repository;

    public GameReadService(IGameRepository repository)
        => this.repository = repository;

    public async Task<Game> GetGameAsync(CancellationToken cancellationToken = default)
    {
        var game = this.repository.Get();

        if (game is null)
        {
            throw new GameNotFoundException();
        }

        var columns = new List<int>();

        for (var column = GameSize.GAME_MIN_SIZE; column <= GameSize.GAME_MAX_SIZE; column++)
        {
            columns.Add(column);
        }

        var rows = CellRowNames.CELL_ROW_MAPPING.Keys.ToList();
        var status = game.Status.ToString();

        var result = new Game
        {
            Columns = columns,
            Rows = rows,
            Size = GameSize.GAME_MAX_SIZE,
            Status = status,
        };

        return await Task.FromResult(result);
    }
}
