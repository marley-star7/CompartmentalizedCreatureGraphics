namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticEyeCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatCosmeticEye);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatCosmeticEye.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new CreatureCosmeticLayerContractResolver<Enums.SlugcatCosmeticLayer>()
        };

        DynamicSlugcatCosmeticEye.Properties properties = JsonConvert.DeserializeObject<DynamicSlugcatCosmeticEye.Properties>(json, settings);
        properties.spriteAngleProperties = CosmeticManager.GetSpriteAnglePropertiesForId(properties.spriteAnglePropertiesId);

        return properties;
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatCosmeticEye(graphicsModule as PlayerGraphics, (DynamicSlugcatCosmeticEye.Properties)GetLoadedPropertiesFromPropertiesId(propertiesId));
    }
}
