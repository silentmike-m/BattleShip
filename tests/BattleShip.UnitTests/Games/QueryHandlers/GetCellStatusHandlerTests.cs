namespace BattleShip.UnitTests.Games.QueryHandlers;

using BattleShip.Application.Games.Queries;
using BattleShip.Infrastructure.Games.Interfaces;
using BattleShip.Infrastructure.Games.QueryHandlers;
using Moq;

[TestClass]
public sealed class GetCellStatusHandlerTests
{
    private const string CELL_STATUS = "Active";
    private const int COLUMN = 1;
    private const char ROW = 'A';

    private readonly NullLogger<GetCellStatusHandler> logger = new();
    private readonly Mock<IGameReadService> readService = new();

    public GetCellStatusHandlerTests()
    {
        this.readService
            .Setup(service => service.GetCellStatusAsync(COLUMN, ROW, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CELL_STATUS);
    }

    [TestMethod]
    public async Task Should_Return_Cell_Status_From_Read_Service()
    {
        //GIVEN
        var request = new GetCellStatus
        {
            Column = COLUMN,
            Row = ROW,
        };

        var handler = new GetCellStatusHandler(this.logger, this.readService.Object);

        //WHEN
        var result = await handler.Handle(request, new CancellationToken());

        //THEN
        result.Should()
            .Be(CELL_STATUS)
            ;
    }
}
