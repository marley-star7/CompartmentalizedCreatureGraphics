namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatTailScalesCosmeticCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatTailScalesCosmetic);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatTailScalesCosmetic.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        return new DynamicSlugcatTailScalesCosmetic.Properties().Parse(json);
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatTailScalesCosmetic(graphicsModule as PlayerGraphics, GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatTailScalesCosmetic.Properties);
    }
}