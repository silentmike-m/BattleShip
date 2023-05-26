namespace BattleShip.Infrastructure.Games.QueryHandlers;

using BattleShip.Application.Games.Queries;
using BattleShip.Application.Games.ViewModel;
using BattleShip.Infrastructure.Games.Interfaces;

internal sealed class GetGameHandler : IRequestHandler<GetGame, Game>
{
    private readonly ILogger<GetGameHandler> logger;
    private readonly IGameReadService readService;

    public GetGameHandler(ILogger<GetGameHandler> logger, IGameReadService readService)
    {
        this.logger = logger;
        this.readService = readService;
    }

    public async Task<Game> Handle(GetGame request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get game");

        var game = await this.readService.GetGameAsync(cancellationToken);

        return game;
    }
}
