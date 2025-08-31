
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
            ContractResolver = new CreatureCosmeticLayerContractResolver<Enums.SlugcatCosmeticLayer>()
        };

        DynamicSlugcatCosmeticEar.Properties properties = JsonConvert.DeserializeObject<DynamicSlugcatCosmeticEar.Properties>(json, settings);
        properties.spriteAngleProperties = CosmeticManager.GetSpriteAnglePropertiesForId(properties.spriteAnglePropertiesId);

        return properties;
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        var playerGraphics = graphicsModule as PlayerGraphics;

        var properties = GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatCosmeticEar.Properties;
        if (properties == null)
        {
            Plugin.LogError($"Properties are null!!!!");
            return null;
        }
        else
            return new DynamicSlugcatCosmeticEar(playerGraphics, properties);
    }
}
