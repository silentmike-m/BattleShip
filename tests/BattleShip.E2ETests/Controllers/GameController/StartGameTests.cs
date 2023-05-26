namespace BattleShip.E2ETests.Controllers.GameController;

using BattleShip.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public sealed class StartGameTests
{
    private const string URL = "Game/StartGame";

    private readonly WebApplicationFactory<Program> factory = new();

    [TestMethod]
    public async Task Should_Create_Game_On_Start_Game()
    {
        //GIVEN
        var client = this.factory.CreateClient();

        //WHEN
        var response = await client.PostAsync(URL, content: null);

        //THEN
        response.EnsureSuccessStatusCode();

        var repository = this.factory.Services.CreateScope().ServiceProvider.GetRequiredService<IGameRepository>();

        var gameResult = repository.Get();

        gameResult.Should()
            .NotBeNull()
            ;
    }
}
