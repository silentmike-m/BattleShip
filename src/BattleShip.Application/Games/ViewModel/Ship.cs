namespace BattleShip.Application.Games.ViewModel;

public sealed record Ship
{
    [JsonPropertyName("hits")] public int Hits { get; init; } = default;
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("size")] public int Size { get; init; } = default;
    [JsonPropertyName("type")] public string Type { get; init; } = string.Empty;
}
