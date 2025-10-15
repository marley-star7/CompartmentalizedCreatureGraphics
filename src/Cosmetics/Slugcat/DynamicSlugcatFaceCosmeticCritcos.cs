namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticFaceCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatFaceCosmetic);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatFaceCosmetic.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        return new DynamicSlugcatFaceCosmetic.Properties().Parse(json);
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatFaceCosmetic(graphicsModule as PlayerGraphics, GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatFaceCosmetic.Properties);
    }
}