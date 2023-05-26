namespace BattleShip.UnitTests.MemoryDb;

using BattleShip.Application.Commons.Constants;
using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.ViewModel;
using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;
using BattleShip.Infrastructure.MemoryDb.Services;

[TestClass]
public sealed class GameReadServiceTests
{
    [TestMethod]
    public async Task Should_Return_Game_On_Get_Game()
    {
        //GIVEN
        var ship = new BattleshipEntity
        {
            Hits = 2,
        };

        var game = new GameEntity
        {
            Fleet = new Dictionary<Guid, ShipEntity>
            {
                { ship.Id, ship },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var service = new GameReadService(repository);

        //WHEN
        var expectedResult = new Game
        {
            Columns = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            Rows = CellRowNames.CELL_ROW_MAPPING.Keys.ToList(),
            Ships = new List<Ship>
            {
                new()
                {
                    Hits = ship.Hits,
                    Name = ship.Name,
                    Size = ship.Size,
                    Type = ship.Type.ToString(),
                },
            },
            Size = GameSize.GAME_MAX_SIZE,
            Status = game.Status.ToString(),
        };

        var result = await service.GetGameAsync();

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [DataTestMethod, DataRow(CellStatus.Hit), DataRow(CellStatus.Miss), DataRow(CellStatus.Unfired)]
    public async Task Should_Return_String_On_Get_Cell_Status(CellStatus status)
    {
        //GIVEN
        const int column = 1;
        const char row = 'A';

        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            Status = status,
        };

        var game = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                cell,
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var service = new GameReadService(repository);

        //WHEN
        var result = await service.GetCellStatusAsync(column, row);

        //THEN
        result.Should()
            .Be(status.ToString())
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Cell_Not_Found_When_Missing_Cell_On_Get_Cell_Status()
    {
        //GIVEN
        const int column = 1;
        const char row = 'A';

        var game = new GameEntity();

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var service = new GameReadService(repository);

        //WHEN
        var action = async () => await service.GetCellStatusAsync(column, row);

        //THEN
        await action.Should()
                .ThrowAsync<CellNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Cell_Not_Found_When_Missing_Row_Number_On_Get_Cell_Status()
    {
        //GIVEN
        const int column = 1;
        const char row = 'Z';

        var game = new GameEntity();

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var service = new GameReadService(repository);

        //WHEN
        var action = async () => await service.GetCellStatusAsync(column, row);

        //THEN
        await action.Should()
                .ThrowAsync<CellNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Game_Not_Found_When_Missing_Game_On_Get_Cell_Status()
    {
        //GIVEN
        const int column = 1;
        const char row = 'A';

        var repository = new GameRepository(new DbContext());

        var service = new GameReadService(repository);

        //WHEN
        var action = async () => await service.GetCellStatusAsync(column, row);

        //THEN
        await action.Should()
                .ThrowAsync<GameNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.GAME_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Game_Not_Found_When_Missing_Game_On_Get_Game()
    {
        //GIVEN
        var repository = new GameRepository(new DbContext());

        var service = new GameReadService(repository);

        //WHEN
        var action = async () => await service.GetGameAsync();

        //THEN
        await action.Should()
                .ThrowAsync<GameNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.GAME_NOT_FOUND)
            ;
    }
}
