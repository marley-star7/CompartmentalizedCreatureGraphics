namespace CompartmentalizedCreatureGraphics.Cosmetics;

public abstract class Critcos : CCGRegisterable
{
    /// <summary>
    /// This should return a dynamic cosmetic with loaded properties from the id, of the correct type.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="propertiesID"></param>
    /// <returns></returns>
    public abstract DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId);
}