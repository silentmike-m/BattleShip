namespace BattleShip.WebApi.ViewModels;

using System.Text.Json.Serialization;

public sealed record BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "ok";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; set; } = default;
}
