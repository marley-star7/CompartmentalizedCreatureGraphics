
namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticFaceCritcos : Critcos
{
    public override DynamicCreatureCosmetic.Properties ParseProperties(Dictionary<string, object> jsonData)
    {
        DynamicCreatureCosmetic.Properties properties = new DynamicCreatureCosmetic.Properties();

        if (MRJson.TryParseFloatProperty(jsonData, nameof(properties.scaleX), out float scaleX))
            properties.scaleX = scaleX;

        if (MRJson.TryParseFloatProperty(jsonData, nameof(properties.scaleY), out float scaleY))
            properties.scaleX = scaleX;

        return properties;
    }

    public override DynamicCreatureCosmetic.Properties GetPropertiesFromPropertiesID(string propertiesID)
    {
        throw new NotImplementedException();
    }
}