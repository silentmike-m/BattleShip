namespace BattleShip.Infrastructure.Games.EventHandlers;

using BattleShip.Application.Games.Commands;
using BattleShip.Application.Games.Events;

public sealed record FiredHandler : INotificationHandler<Fired>
{
    private readonly ILogger<FiredHandler> logger;
    private readonly ISender mediator;

    public FiredHandler(ILogger<FiredHandler> logger, ISender mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(Fired notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to send finish game after fired");

        var request = new TryToFinishGame();

        await this.mediator.Send(request, cancellationToken);
    }
}
