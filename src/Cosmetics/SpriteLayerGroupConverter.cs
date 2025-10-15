namespace CompartmentalizedCreatureGraphics.Cosmetics;

public class SpriteLayerGroupConverter<TEnum> : JsonConverter<SpriteLayerGroup> where TEnum : struct, Enum
{
    private readonly EnumToIntJsonConverter<TEnum> _enumConverter = new EnumToIntJsonConverter<TEnum>();

    public override void WriteJson(JsonWriter writer, SpriteLayerGroup value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        // Write layerName using the enum converter
        writer.WritePropertyName("layerName");
        _enumConverter.WriteJson(writer, value.Layer, serializer);

        writer.WritePropertyName("startSpriteIndex");
        writer.WriteValue(value.StartSpriteIndex);

        writer.WritePropertyName("endSpriteIndex");
        writer.WriteValue(value.EndSpriteIndex);

        writer.WriteEndObject();
    }

    public override SpriteLayerGroup ReadJson(JsonReader reader, Type objectType, SpriteLayerGroup existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.StartObject)
        {
            throw new JsonSerializationException($"Expected StartObject, found {reader.TokenType}");
        }

        int layer = 0;
        int startSpriteIndex = 0;
        int endSpriteIndex = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
                break;

            if (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value?.ToString();
                reader.Read(); // Move to value

                switch (propertyName)
                {
                    case "layerName":
                        layer = (int)_enumConverter.ReadJson(reader, typeof(int), null, serializer);
                        break;
                    case "startSpriteIndex":
                        startSpriteIndex = reader.Value != null ? Convert.ToInt32(reader.Value) : 0;
                        break;
                    case "endSpriteIndex":
                        endSpriteIndex = reader.Value != null ? Convert.ToInt32(reader.Value) : 0;
                        break;
                        // Ignore other properties like needsReorder during deserialization
                }
            }
        }

        return new SpriteLayerGroup(layer, startSpriteIndex, endSpriteIndex);
    }
}