namespace CompartmentalizedCreatureGraphics.Effects;

public abstract class CritcosEffect : CCGRegisterable
{
    /// <summary>
    /// This should return a dynamic cosmetic with loaded properties from the id, of the correct type.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="propertiesID"></param>
    /// <returns></returns>
    public abstract DynamicCosmeticEffect CreateEffectForDynamicCosmetic(DynamicCreatureCosmetic creatureCosmetic, string propertiesId);
}