
namespace CompartmentalizedCreatureGraphics.Effects;

public class ColorSpriteCritcosEffect : CritcosEffect
{
    public override Type DynamicCreatureCosmeticType => typeof(ColorSpriteCosmeticEffect);

    public override Type DynamicCreatureCosmeticPropertiesType => typeof(ColorSpriteCosmeticEffect.Properties);

    public override DynamicCosmeticEffect CreateEffectForDynamicCosmetic(DynamicCreatureCosmetic creatureCosmetic, string propertiesId)
    {
        throw new NotImplementedException();
    }

    public override DynamicCreatureCosmetic.Properties ParseProperties(string json)
    {
        throw new NotImplementedException();
    }
}
