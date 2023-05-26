namespace BattleShip.E2ETests.Controllers.GameController;

using System.Net;
using System.Net.Http.Json;
using BattleShip.Application.Commons.Constants;
using BattleShip.Application.Commons.Extensions;
using BattleShip.Application.Exceptions;
using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;
using BattleShip.Domain.Repositories;
using BattleShip.WebApi.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public sealed class FireTests
{
    private const string URL = "Game/Fire";

    private readonly WebApplicationFactory<Program> factory = new();

    [TestMethod]
    public async Task Should_Mark_Cell_And_Ship_As_Hit_And_Finish_Game_On_Fire()
    {
        //GIVEN
        var activeShip = new BattleshipEntity();
        activeShip.Hits = activeShip.Size - 1;

        var activeCell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = activeShip.Id,
            Status = CellStatus.Unfired,
        };

        var activeGame = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                activeCell,
            },
            Fleet = new Dictionary<Guid, Ship>
            {
                { activeShip.Id, activeShip },
            },
        };

        var repository = this.factory.Services.CreateScope().ServiceProvider.GetRequiredService<IGameRepository>();
        repository.Save(activeGame);

        var request = new Fire
        {
            Column = 1,
            Row = 'A',
        };

        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.PostAsJsonAsync(URL, request);

        //THEN
        response.EnsureSuccessStatusCode();

        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(activeGame, options => options
                .Excluding(game => game.Cells)
                .Excluding(game => game.Fleet)
                .Excluding(game => game.Status)
            )
            ;

        gameResult!.Status.Should()
            .Be(GameStatus.Finished)
            ;

        gameResult.Cells.Should()
            .HaveCount(1)
            ;

        gameResult.Cells[0].Should()
            .BeEquivalentTo(activeCell, options => options.Excluding(cell => cell.Status))
            ;

        gameResult.Cells[0].Status.Should()
            .Be(CellStatus.Hit)
            ;

        gameResult.Fleet.Should()
            .HaveCount(1)
            .And
            .ContainKey(activeShip.Id)
            ;

        gameResult.Fleet[activeShip.Id].Should()
            .BeEquivalentTo(activeShip, options => options.Excluding(ship => ship.Hits))
            ;

        gameResult.Fleet[activeShip.Id].Hits.Should()
            .Be(activeShip.Hits + 1)
            ;
    }

    [TestMethod]
    public async Task Should_Mark_Cell_And_Ship_As_Hit_On_Fire()
    {
        //GIVEN
        var activeShip = new BattleshipEntity();

        var activeCell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = activeShip.Id,
            Status = CellStatus.Unfired,
        };

        var activeGame = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                activeCell,
            },
            Fleet = new Dictionary<Guid, Ship>
            {
                { activeShip.Id, activeShip },
            },
        };

        var repository = this.factory.Services.CreateScope().ServiceProvider.GetRequiredService<IGameRepository>();
        repository.Save(activeGame);

        var request = new Fire
        {
            Column = 1,
            Row = 'A',
        };

        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.PostAsJsonAsync(URL, request);

        //THEN
        response.EnsureSuccessStatusCode();

        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(activeGame, options => options
                .Excluding(game => game.Cells)
                .Excluding(game => game.Fleet)
            )
            ;

        gameResult!.Cells.Should()
            .HaveCount(1)
            ;

        gameResult.Cells[0].Should()
            .BeEquivalentTo(activeCell, options => options.Excluding(cell => cell.Status))
            ;

        gameResult.Cells[0].Status.Should()
            .Be(CellStatus.Hit)
            ;

        gameResult.Fleet.Should()
            .HaveCount(1)
            .And
            .ContainKey(activeShip.Id)
            ;

        gameResult.Fleet[activeShip.Id].Should()
            .BeEquivalentTo(activeShip, options => options.Excluding(ship => ship.Hits))
            ;

        gameResult.Fleet[activeShip.Id].Hits.Should()
            .Be(activeShip.Hits + 1)
            ;
    }

    [TestMethod]
    public async Task Should_Mark_Cell_As_Miss_When_No_Ship_On_Fire()
    {
        //GIVEN
        var ship = new BattleshipEntity();

        var cell = new CellEntity
        {
            Column = 1,
            Row = 1,
            ShipId = null,
            Status = CellStatus.Unfired,
        };

        var activeGame = new GameEntity
        {
            Cells = new List<CellEntity>
            {
                cell,
            },
            Fleet = new Dictionary<Guid, Ship>
            {
                { ship.Id, ship },
            },
        };

        var repository = this.factory.Services.CreateScope().ServiceProvider.GetRequiredService<IGameRepository>();
        repository.Save(activeGame);

        var request = new Fire
        {
            Column = 1,
            Row = 'A',
        };

        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.PostAsJsonAsync(URL, request);

        //THEN
        response.EnsureSuccessStatusCode();

        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(activeGame, options => options.Excluding(game => game.Cells))
            ;

        gameResult!.Cells.Should()
            .HaveCount(1)
            ;

        gameResult.Cells[0].Status.Should()
            .Be(CellStatus.Miss)
            ;
    }

    [TestMethod]
    public async Task Should_Return_Validation_Exception_When_Row_Is_No_Correct_On_Fire()
    {
        //GIVEN
        var request = new Fire
        {
            Column = 1,
            Row = 'Z',
        };

        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.PostAsJsonAsync(URL, request);

        //THEN
        response.StatusCode.Should()
            .Be(HttpStatusCode.InternalServerError)
            ;

        var responseToString = await response.Content.ReadAsStringAsync();

        var expectedFailure = new ValidationFailure(nameof(Fire.Row), ValidationErrorCodes.FIRE_INVALID_ROW_MESSAGE)
        {
            ErrorCode = ValidationErrorCodes.FIRE_INVALID_ROW,
        };

        var expectedException = new ValidationException(new List<ValidationFailure> { expectedFailure });

        var expectedResult = new BaseResponse<IDictionary<string, string[]>>
        {
            Code = expectedException.Code,
            Error = expectedException.Message,
            Response = expectedException.Errors,
        };

        var result = responseToString.To<BaseResponse<IDictionary<string, string[]>>>();

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }
}
