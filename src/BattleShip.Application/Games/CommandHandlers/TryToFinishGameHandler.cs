namespace BattleShip.Application.Games.CommandHandlers;

using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Repositories;

internal sealed class TryToFinishGameHandler : IRequestHandler<TryToFinishGame>
{
    private readonly ILogger<TryToFinishGameHandler> logger;
    private readonly IGameRepository repository;

    public TryToFinishGameHandler(ILogger<TryToFinishGameHandler> logger, IGameRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(TryToFinishGame request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to finish game");

        var game = this.repository.Get();

        if (game is null)
        {
            throw new GameNotFoundException();
        }

        var activeShip = game.Fleet.Values.FirstOrDefault(ship => ship.Size > ship.Hits);

        if (activeShip is null)
        {
            game.Status = GameStatus.Finished;

            this.repository.Save(game);
        }

        await Task.CompletedTask;
    }
}
