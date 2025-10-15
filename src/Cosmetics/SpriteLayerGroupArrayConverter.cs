namespace CompartmentalizedCreatureGraphics.Cosmetics;

public class SpriteLayerGroupArrayConverter<TEnum> : JsonConverter<SpriteLayerGroup[]> where TEnum : struct, Enum
{
    private readonly SpriteLayerGroupConverter<TEnum> _itemConverter = new SpriteLayerGroupConverter<TEnum>();

    public override void WriteJson(JsonWriter writer, SpriteLayerGroup[] value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (var group in value)
        {
            _itemConverter.WriteJson(writer, group, serializer);
        }
        writer.WriteEndArray();
    }

    public override SpriteLayerGroup[] ReadJson(JsonReader reader, Type objectType, SpriteLayerGroup[] existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartArray)
        {
            throw new JsonSerializationException($"Expected StartArray, found {reader.TokenType}");
        }

        var groups = new List<SpriteLayerGroup>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndArray)
                break;

            if (reader.TokenType == JsonToken.StartObject)
            {
                var group = (SpriteLayerGroup) _itemConverter.ReadJson(reader, typeof(SpriteLayerGroup), default(SpriteLayerGroup), serializer);
                groups.Add(group);
            }
        }

        return groups.ToArray();
    }
}