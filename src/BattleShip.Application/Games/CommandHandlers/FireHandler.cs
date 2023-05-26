namespace BattleShip.Application.Games.CommandHandlers;

using BattleShip.Application.Games.Commands;
using BattleShip.Domain.Repositories;

internal sealed class FireHandler : IRequestHandler<Fire>
{
    private readonly ILogger<FireHandler> logger;
    private readonly IGameRepository repository;

    public FireHandler(ILogger<FireHandler> logger, IGameRepository repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task Handle(Fire request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to process fire at column '{Column}' and row '{Row}'", request.Column, request.Row);

        await Task.CompletedTask;
    }
}
