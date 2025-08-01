
namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticEarCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatCosmeticEar);

    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatCosmeticEar.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Error
        };

        DynamicSlugcatCosmeticEar.Properties properties = JsonConvert.DeserializeObject<DynamicSlugcatCosmeticEar.Properties>(json, settings);
        return properties;
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForPlayer(Player player, string propertiesId)
    {
        var properties = GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatCosmeticEar.Properties;
        if (properties == null)
        {
            Plugin.LogError($"Properties are null!!!!");
            return null;
        }
        else
            return new DynamicSlugcatCosmeticEar(player, properties);
    }
}
