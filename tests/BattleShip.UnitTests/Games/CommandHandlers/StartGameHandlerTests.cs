namespace BattleShip.UnitTests.Games.CommandHandlers;

using BattleShip.Application.Games.CommandHandlers;
using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Enums;
using BattleShip.Infrastructure.MemoryDb.Services;

[TestClass]
public sealed class StartGameHandlerTests
{
    private const int FLEET_MAX_SIZE = 3;
    private const GameStatus NEW_GAME_STATUS = GameStatus.Active;

    private readonly NullLogger<StartGameHandler> logger = new();

    [TestMethod]
    public async Task Should_Create_Game()
    {
        //GIVEN
        var repository = new GameRepository(new DbContext());

        var request = new StartGame();

        var handler = new StartGameHandler(this.logger, repository);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        var result = repository.Get();

        result.Should()
            .NotBeNull()
            ;

        result!.Cells.Should()
            .HaveCount(GameSize.GAME_MAX_SIZE * GameSize.GAME_MAX_SIZE)
            ;

        result.Fleet.Should()
            .HaveCount(FLEET_MAX_SIZE)
            ;

        result.Fleet.Values.Where(ship => ship.Type == ShipType.Battleship).Should()
            .HaveCount(1)
            ;

        result.Fleet.Values.Where(ship => ship.Type == ShipType.Destroyer).Should()
            .HaveCount(2)
            ;

        result.Id.Should()
            .NotBeEmpty()
            ;

        result.Status.Should()
            .Be(NEW_GAME_STATUS)
            ;

        foreach (var (_, ship) in result.Fleet)
        {
            var shipCells = result.Cells
                .Where(cell => cell.ShipId == ship.Id)
                .OrderBy(cell => cell.Row)
                .ThenBy(cell => cell.Column)
                .ToList();

            shipCells.Should()
                .HaveCount(ship.Size)
                ;

            if (ship.IsHorizontalOrientation)
            {
                var row = shipCells[0].Row;
                var firstColumn = shipCells[0].Column;

                for (var column = 1; column < ship.Size; column++)
                {
                    shipCells[column].Row.Should()
                        .Be(row);

                    shipCells[column].Column.Should()
                        .Be(firstColumn + column);
                }
            }
            else
            {
                var column = shipCells[0].Column;
                var firstRow = shipCells[0].Row;

                for (var row = 1; row < ship.Size; row++)
                {
                    shipCells[row].Row.Should()
                        .Be(firstRow + row);

                    shipCells[row].Column.Should()
                        .Be(column);
                }
            }
        }
    }
}
