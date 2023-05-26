namespace BattleShip.Application.Games.Queries;

public sealed record GetCellStatus : IRequest<string>
{
    public int Column { get; init; } = default;
    public char Row { get; init; } = default;
}
