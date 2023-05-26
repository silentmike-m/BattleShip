namespace BattleShip.Infrastructure.MemoryDb.Converters;

using System.Text.Json;
using BattleShip.Domain.Entities;
using BattleShip.Domain.Enums;
using BattleShip.Domain.Interfaces;

internal sealed class ShipConverter : JsonConverter<ShipEntity>
{
    public override ShipEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ShipEntity? result;

        using var document = JsonDocument.ParseValue(ref reader);

        var shipType = document.RootElement.GetProperty(nameof(ShipEntity.Type)).GetString();

        if (shipType == ShipType.Battleship.ToString())
        {
            result = document.RootElement.Deserialize<BattleshipEntity>(options);
        }
        else
        {
            result = document.RootElement.Deserialize<DestroyerEntity>(options);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, ShipEntity value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (ShipEntity?)null, options);

                break;
            default:
            {
                var type = value.GetType();
                JsonSerializer.Serialize(writer, value, type, options);

                break;
            }
        }
    }
}
