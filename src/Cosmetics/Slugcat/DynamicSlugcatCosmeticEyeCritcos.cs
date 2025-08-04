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
            MissingMemberHandling = MissingMemberHandling.Error,
            ContractResolver = new CreatureCosmeticLayerContractResolver<CCGEnums.SlugcatCosmeticLayer>()
        };

        DynamicSlugcatCosmeticEye.Properties properties = JsonConvert.DeserializeObject<DynamicSlugcatCosmeticEye.Properties>(json, settings);
        return properties;
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatCosmeticEye(graphicsModule as PlayerGraphics, (DynamicSlugcatCosmeticEye.Properties)GetLoadedPropertiesFromPropertiesId(propertiesId));
    }
}
