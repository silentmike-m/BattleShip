namespace BattleShip.UnitTests.Games.CommandHandlers;

using BattleShip.Application.Commons.Constants;
using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.CommandHandlers;
using BattleShip.Application.Games.Commands;
using BattleShip.Application.Games.Events;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;
using BattleShip.Infrastructure.MemoryDb.Services;
using MediatR;
using Moq;

[TestClass]
public sealed class FireHandlerTests
{
    private readonly NullLogger<FireHandler> logger = new();
    private readonly Mock<IPublisher> mediator = new();

    [TestMethod]
    public async Task Should_Hit_On_Fire()
    {
        //GIVEN
        Fired? firedNotification = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<Fired>(), It.IsAny<CancellationToken>()))
            .Callback<Fired, CancellationToken>((notification, _) => firedNotification = notification);

        var ship = new BattleshipEntity();

        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = ship.Id,
            Status = CellStatus.Unfired,
        };

        var game = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                cell,
            },
            Fleet = new Dictionary<Guid, ShipEntity>
            {
                { ship.Id, ship },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var request = new Fire
        {
            Column = cell.Column,
            Row = 'A',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            ;

        gameResult!.Cells[0].Status.Should()
            .Be(CellStatus.Hit)
            ;

        gameResult.Fleet[ship.Id].Hits.Should()
            .Be(1)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<Fired>(), It.IsAny<CancellationToken>()), Times.Once);

        firedNotification.Should()
            .NotBeNull()
            ;
    }

    [TestMethod]
    public async Task Should_Miss_On_Fire()
    {
        //GIVEN
        Fired? firedNotification = null;

        this.mediator
            .Setup(service => service.Publish(It.IsAny<Fired>(), It.IsAny<CancellationToken>()))
            .Callback<Fired, CancellationToken>((notification, _) => firedNotification = notification);

        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = null,
            Status = CellStatus.Unfired,
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

        var request = new Fire
        {
            Column = cell.Column,
            Row = 'A',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            ;

        gameResult!.Cells[0].Status.Should()
            .Be(CellStatus.Miss)
            ;

        this.mediator.Verify(service => service.Publish(It.IsAny<Fired>(), It.IsAny<CancellationToken>()), Times.Once);

        firedNotification.Should()
            .NotBeNull()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Already_Fired_When_Cell_Has_Been_Hit_On_Fire()
    {
        //GIVEN
        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = Guid.NewGuid(),
            Status = CellStatus.Hit,
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

        var request = new Fire
        {
            Column = cell.Column,
            Row = 'A',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CellAlreadyFiredException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_ALREADY_FIRED)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Already_Fired_When_Cell_Has_Been_Missed_On_Fire()
    {
        //GIVEN
        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = null,
            Status = CellStatus.Miss,
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

        var request = new Fire
        {
            Column = cell.Column,
            Row = 'A',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CellAlreadyFiredException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_ALREADY_FIRED)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Cell_Not_Found_When_Missing_Cell_With_Column_On_Fire()
    {
        //GIVEN
        var game = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                new()
                {
                    Column = 1,
                    Row = 'A',
                },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var request = new Fire
        {
            Column = 2,
            Row = 'A',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CellNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Cell_Not_Found_When_Missing_Cell_With_Row_On_Fire()
    {
        //GIVEN
        var game = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                new()
                {
                    Column = 1,
                    Row = 'A',
                },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var request = new Fire
        {
            Column = 1,
            Row = 'B',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CellNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Cell_Not_Found_When_Row_Number_Is_Null_On_Fire()
    {
        //GIVEN
        var game = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                new()
                {
                    Column = 1,
                    Row = 'A',
                },
            },
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var request = new Fire
        {
            Column = 1,
            Row = 'Z',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CellNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.CELL_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Game_Finished_When_Game_Is_Finished_on_Fire()
    {
        //GIVEN
        var game = new GameEntity
        {
            Status = GameStatus.Finished,
        };

        var repository = new GameRepository(new DbContext());
        repository.Save(game);

        var request = new Fire();

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<GameFinishedException>()
                .Where(exception => exception.Code == ErrorCodes.GAME_FINISHED)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Game_Not_Found_When_Missing_Game_On_Fire()
    {
        //GIVEN
        var repository = new GameRepository(new DbContext());

        var request = new Fire();

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<GameNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.GAME_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Ship_Not_Found_When_Missing_Cell_Ship_On_Fire()
    {
        //GIVEN
        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = Guid.NewGuid(),
            Status = CellStatus.Unfired,
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

        var request = new Fire
        {
            Column = cell.Column,
            Row = 'A',
        };

        var handler = new FireHandler(this.logger, this.mediator.Object, repository);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<ShipNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.SHIP_NOT_FOUND)
            ;
    }
}
