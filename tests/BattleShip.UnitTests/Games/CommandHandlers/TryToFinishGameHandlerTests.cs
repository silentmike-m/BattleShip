namespace BattleShip.UnitTests.Games.CommandHandlers;

using BattleShip.Application.Commons.Constants;
using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.CommandHandlers;
using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;
using BattleShip.Infrastructure.MemoryDb.Services;

[TestClass]
public sealed class TryToFinishGameHandlerTests
{
    private readonly NullLogger<TryToFinishGameHandler> logger = new();

    [TestMethod]
    public async Task Should_Change_Game_Status_When_Missing_Active_Ship_On_Change_Game_Status()
    {
        //GIVEN
        var ship = new BattleshipEntity();
        ship.Hits = ship.Size;

        var activeGame = new GameEntity
        {
            Fleet = new Dictionary<Guid, ShipEntity>
            {
                { ship.Id, ship },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(activeGame);

        var request = new TryToFinishGame();

        var handler = new TryToFinishGameHandler(this.logger, repository);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(activeGame, options => options.Excluding(game => game.Status))
            ;

        gameResult!.Status.Should()
            .Be(GameStatus.Finished)
            ;
    }

    [TestMethod]
    public async Task Should_Not_Change_Game_Status_When_Ship_Is_Active_On_Change_Game_Status()
    {
        //GIVEN
        var ship = new BattleshipEntity();

        var activeGame = new GameEntity
        {
            Fleet = new Dictionary<Guid, ShipEntity>
            {
                { ship.Id, ship },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(activeGame);

        var request = new TryToFinishGame();

        var handler = new TryToFinishGameHandler(this.logger, repository);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(activeGame)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Game_Not_Found_When_Missing_Game_On_Change_Game_Status()
    {
        //GIVEN
        var repository = new GameRepository(new DbContext());

        var request = new TryToFinishGame();

        var handler = new TryToFinishGameHandler(this.logger, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<GameNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.GAME_NOT_FOUND)
            ;
    }
}
