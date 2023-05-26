namespace BattleShip.Application.Games.Commands;

public sealed record Fire : IRequest
{
    [JsonPropertyName("column")] public int Column { get; init; } = default;
    [JsonPropertyName("row")] public char Row { get; init; } = default;
}
