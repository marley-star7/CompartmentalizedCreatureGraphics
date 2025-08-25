
namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticFaceCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatFaceCosmetic);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatFaceCosmetic.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new CreatureCosmeticLayerContractResolver<CCGEnums.SlugcatCosmeticLayer>()
        };

        DynamicSlugcatFaceCosmetic.Properties properties = JsonConvert.DeserializeObject<DynamicSlugcatFaceCosmetic.Properties>(json, settings);
        properties.spriteAngleProperties = CosmeticManager.GetSpriteAnglePropertiesForId(properties.spriteAnglePropertiesId);

        return properties;
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatFaceCosmetic(graphicsModule as PlayerGraphics, GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatFaceCosmetic.Properties);
    }
}