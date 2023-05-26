namespace BattleShip.Infrastructure.Games.QueryHandlers;

using BattleShip.Application.Games.Queries;
using BattleShip.Infrastructure.Games.Interfaces;

internal sealed class GetCellStatusHandler : IRequestHandler<GetCellStatus, string>
{
    private readonly ILogger<GetCellStatusHandler> logger;
    private readonly IGameReadService readService;

    public GetCellStatusHandler(ILogger<GetCellStatusHandler> logger, IGameReadService readService)
    {
        this.logger = logger;
        this.readService = readService;
    }

    public async Task<string> Handle(GetCellStatus request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to get cell status with column '{Column}' and row '{Row}'", request.Column, request.Row);

        var result = await this.readService.GetCellStatusAsync(request.Column, request.Row, cancellationToken);

        return result;
    }
}
