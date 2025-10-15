namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatTailSpecksCosmeticCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatTailSpecksCosmetic);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatTailSpecksCosmetic.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        return new DynamicSlugcatTailSpecksCosmetic.Properties().Parse(json);
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatTailSpecksCosmetic(graphicsModule as PlayerGraphics, GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatTailSpecksCosmetic.Properties);
    }
}