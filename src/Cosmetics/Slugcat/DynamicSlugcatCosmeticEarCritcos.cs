namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticEarCritcos : Critcos
{
    public override Type DynamicCreatureCosmeticType => typeof(DynamicSlugcatCosmeticEar);
    public override Type DynamicCreatureCosmeticPropertiesType => typeof(DynamicSlugcatCosmeticEar.Properties);

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        return new DynamicSlugcatCosmeticEar.Properties().Parse(json);
    }

    public override DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId)
    {
        var playerGraphics = graphicsModule as PlayerGraphics;

        var properties = GetLoadedPropertiesFromPropertiesId(propertiesId) as DynamicSlugcatCosmeticEar.Properties;
        if (properties == null)
        {
            Plugin.LogCCGError($"Properties are null!!!!");
            return null;
        }
        else
            return new DynamicSlugcatCosmeticEar(playerGraphics, properties);
    }
}
