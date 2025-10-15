namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public sealed class DynamicSlugcatCosmeticEyeCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatCosmeticEye);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatCosmeticEye.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        return new DynamicSlugcatCosmeticEye.Properties().Parse(json);
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        return new DynamicSlugcatCosmeticEye(graphicsModule as PlayerGraphics, (DynamicSlugcatCosmeticEye.Properties)GetLoadedPropertiesFromPropertiesId(propertiesId));
    }
}
