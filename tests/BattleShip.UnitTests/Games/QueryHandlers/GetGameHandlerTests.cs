namespace BattleShip.UnitTests.Games.QueryHandlers;

using BattleShip.Application.Games.Queries;
using BattleShip.Application.Games.ViewModel;
using BattleShip.Infrastructure.Games.Interfaces;
using BattleShip.Infrastructure.Games.QueryHandlers;
using Moq;

[TestClass]
public sealed class GetGameHandlerTests
{
    private static readonly Game GAME = new()
    {
        Columns = new List<int> { 1, 2, 3 },
        Rows = new List<char> { 'A', 'B', 'C' },
        Size = 3,
        Status = "Active",
    };

    private readonly NullLogger<GetGameHandler> logger = new();
    private readonly Mock<IGameReadService> readService = new();

    public GetGameHandlerTests()
    {
        this.readService
            .Setup(service => service.GetGameAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(GAME);
    }

    [TestMethod]
    public async Task Should_Return_Game_From_Read_Service()
    {
        //GIVEN
        var request = new GetGame();

        var handler = new GetGameHandler(this.logger, this.readService.Object);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .BeEquivalentTo(GAME)
            ;
    }
}
