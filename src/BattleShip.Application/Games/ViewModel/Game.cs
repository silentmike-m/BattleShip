namespace BattleShip.Application.Games.ViewModel;

public sealed record Game
{
    [JsonPropertyName("columns")] public IReadOnlyList<int> Columns { get; init; } = new List<int>();
    [JsonPropertyName("rows")] public IReadOnlyList<char> Rows { get; init; } = new List<char>();
    [JsonPropertyName("ships")] public IReadOnlyList<Ship> Ships { get; init; } = new List<Ship>();
    [JsonPropertyName("size")] public int Size { get; init; } = default;
    [JsonPropertyName("status")] public string Status { get; init; } = string.Empty;
}
