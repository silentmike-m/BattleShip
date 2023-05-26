namespace BattleShip.E2ETests.Controllers.GameController;

using System.Net;
using BattleShip.Application.Commons.Extensions;
using BattleShip.Application.Exceptions.Games;
using BattleShip.Application.Games.ViewModel;
using BattleShip.Domain.Common.Constants;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Interfaces;
using BattleShip.Domain.Repositories;
using BattleShip.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public sealed record GetGameTests
{
    private const string URL = "Game/GetGame";

    private readonly WebApplicationFactory<Program> factory = new();

    [TestMethod]
    public async Task Should_Return_Game_Not_Found_When_Missing_Game_On_Get_Game()
    {
        //GIVEN
        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.GetAsync(URL);

        //THEN
        response.StatusCode.Should()
            .Be(HttpStatusCode.InternalServerError)
            ;

        var responseToString = await response.Content.ReadAsStringAsync();

        var expectedException = new GameNotFoundException();

        var expectedResult = new BaseResponse<Game>
        {
            Code = expectedException.Code,
            Error = expectedException.Message,
        };

        var result = responseToString.To<BaseResponse<Game>>();

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }

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

        var repository = this.factory.Services.CreateScope().ServiceProvider.GetRequiredService<IGameRepository>();
        repository.Save(game);

        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.GetAsync(URL);

        //THEN
        response.EnsureSuccessStatusCode();

        var responseToString = await response.Content.ReadAsStringAsync();

        var expectedResult = new BaseResponse<Game>
        {
            Response = new Game
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
            },
        };

        var result = responseToString.To<BaseResponse<Game>>();

        result.Should()
            .BeEquivalentTo(expectedResult)
            ;
    }
}
