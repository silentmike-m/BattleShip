namespace BattleShip.Application.Games.CommandHandlers;

using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.Commands;
using BattleShip.Application.Games.Events;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Extensions;
using BattleShip.Domain.Repositories;

internal sealed class FireHandler : IRequestHandler<Fire>
{
    private readonly ILogger<FireHandler> logger;
    private readonly IPublisher mediator;
    private readonly IGameRepository repository;

    public FireHandler(ILogger<FireHandler> logger, IPublisher mediator, IGameRepository repository)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.repository = repository;
    }

    public async Task Handle(Fire request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to process fire at column '{Column}' and row '{Row}'", request.Column, request.Row);

        var game = this.repository.Get();

        if (game is null)
        {
            throw new GameNotFoundException();
        }

        if (game.Status is GameStatus.Finished)
        {
            throw new GameFinishedException();
        }

        var cell = GetCell(request.Column, game, request.Row);

        switch (cell.Status)
        {
            case CellStatus.Unfired:
                MarkCell(cell, game);

                break;
            case CellStatus.Hit or CellStatus.Miss:
                throw new CellAlreadyFiredException(cell.Column, request.Row, cell.Status.ToString());
        }

        this.repository.Save(game);

        var notification = new Fired();

        await this.mediator.Publish(notification, cancellationToken);

        await Task.CompletedTask;
    }

    private static CellEntity GetCell(int column, GameEntity game, char row)
    {
        var rowNumber = row.GetRowNumber();

        if (rowNumber is null)
        {
            throw new CellNotFoundException(column, row);
        }

        var cell = game.Cells.Get(column, rowNumber.Value);

        if (cell is null)
        {
            throw new CellNotFoundException(column, row);
        }

        return cell;
    }

    private static void MarkCell(CellEntity cell, GameEntity game)
    {
        if (cell.ShipId is null)
        {
            cell.Status = CellStatus.Miss;
        }
        else
        {
            if (game.Fleet.TryGetValue(cell.ShipId.Value, out var ship) is false)
            {
                throw new ShipNotFoundException(cell.ShipId.Value);
            }

            cell.Status = CellStatus.Hit;
            ship.Hits += 1;
        }
    }
}
