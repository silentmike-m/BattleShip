namespace BattleShip.UnitTests.Games.EventHandlers;

using BattleShip.Application.Games.Commands;
using BattleShip.Application.Games.Events;
using BattleShip.Infrastructure.Games.EventHandlers;
using MediatR;
using Moq;

[TestClass]
public sealed class FiredHandlerTests
{
    private readonly NullLogger<FiredHandler> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Send_Change_Game_Status_On_Fired()
    {
        //GIVEN
        TryToFinishGame? changeGameStatus = null;

        this.mediator
            .Setup(service => service.Send(It.IsAny<TryToFinishGame>(), It.IsAny<CancellationToken>()))
            .Callback<TryToFinishGame, CancellationToken>((request, _) => changeGameStatus = request);

        var notification = new Fired();

        var handler = new FiredHandler(this.logger, this.mediator.Object);

        //WHEN
        await handler.Handle(notification, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<TryToFinishGame>(), It.IsAny<CancellationToken>()), Times.Once);

        changeGameStatus.Should()
            .NotBeNull()
            ;
    }
}
