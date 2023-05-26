namespace BattleShip.Application.Commons.Extensions;

using System.Text.Json;

public static class JsonExtension
{
    public static T To<T>(this string source, params JsonConverter[]? converters)
    {
        var options = GetOptions(converters);

        return JsonSerializer.Deserialize<T>(source, options)
               ?? throw new NullReferenceException();
    }

    public static string ToJson<T>(this T source, params JsonConverter[]? converters)
    {
        var options = GetOptions(converters);

        return JsonSerializer.Serialize(source, options);
    }

    private static JsonSerializerOptions GetOptions(params JsonConverter[]? converters)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        if (converters is not null)
        {
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }
        }

        return options;
    }
}
