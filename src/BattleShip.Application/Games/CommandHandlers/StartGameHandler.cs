namespace BattleShip.Application.Games.CommandHandlers;

using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Extensions;
using BattleShip.Domain.Interfaces;
using BattleShip.Domain.Repositories;

internal sealed class StartGameHandler : IRequestHandler<StartGame>
{
    private readonly ILogger<StartGameHandler> logger;
    private readonly Random random;
    private readonly IGameRepository repository;

    public StartGameHandler(ILogger<StartGameHandler> logger, IGameRepository repository)
    {
        this.random = new Random(Guid.NewGuid().GetHashCode());
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(StartGame request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to start game");

        var game = this.repository.Get();

        if (game is not null)
        {
            this.logger.LogDebug("Game with id {GameId} with status {GameStatus} already exists. It will be overwritten", game.Id, game.Status);
        }

        var cells = CreateCells();
        var fleet = CreateFleet();

        this.PlaceFleet(cells, fleet);

        game = new GameEntity
        {
            Cells = cells,
            Fleet = fleet,
            Status = GameStatus.Active,
        };

        this.repository.Save(game);

        await Task.CompletedTask;
    }

    private void PlaceFleet(IReadOnlyList<CellEntity> cells, Dictionary<Guid, Ship> fleet)
    {
        //TODO: stop loop

        foreach (var (_, ship) in fleet)
        {
            while (true)
            {
                var isShipPlaced = this.TryToPlaceShip(cells, ship);

                if (isShipPlaced)
                {
                    break;
                }
            }
        }
    }

    private bool TryToPlaceShip(IReadOnlyList<CellEntity> cells, Ship ship)
    {
        var isHorizontalOrientation = IsHorizontalOrientation(this.random);
        var startColumn = this.random.Next(GameSize.GAME_MIN_SIZE, GameSize.GAME_MAX_SIZE);
        var startRow = this.random.Next(GameSize.GAME_MIN_SIZE, GameSize.GAME_MAX_SIZE);

        var endColumn = isHorizontalOrientation
            ? startColumn + ship.Size - 1
            : startColumn;

        var endRow = isHorizontalOrientation
            ? startRow
            : startRow + ship.Size - 1;

        if (endColumn > GameSize.GAME_MAX_SIZE || endRow > GameSize.GAME_MAX_SIZE)
        {
            return false;
        }

        var range = cells
            .GetRange(endColumn, endRow, startColumn, startRow)
            .ToList();

        var cellWithShip = range.FirstOrDefault(cell => cell.ShipId is not null);

        if (cellWithShip is not null)
        {
            return false;
        }

        foreach (var cell in range)
        {
            cell.ShipId = ship.Id;
        }

        ship.IsHorizontalOrientation = isHorizontalOrientation;

        return true;
    }

    private static IReadOnlyList<CellEntity> CreateCells()
    {
        var result = new List<CellEntity>();

        for (var rowNumber = GameSize.GAME_MIN_SIZE; rowNumber <= GameSize.GAME_MAX_SIZE; rowNumber++)
        {
            for (var columnNumber = GameSize.GAME_MIN_SIZE; columnNumber <= GameSize.GAME_MAX_SIZE; columnNumber++)
            {
                var cell = new CellEntity
                {
                    Column = columnNumber,
                    Row = rowNumber,
                };

                result.Add(cell);
            }
        }

        return result;
    }

    private static Dictionary<Guid, Ship> CreateFleet()
    {
        var fleet = new List<Ship>
        {
            new BattleshipEntity(),
            new DestroyerEntity(),
            new DestroyerEntity(),
        };

        return fleet.ToDictionary(ship => ship.Id, ship => ship);
    }

    private static bool IsHorizontalOrientation(Random random)
    {
        var orientation = random.Next(minValue: 1, maxValue: 101) % 2;

        return orientation == 0;
    }
}
