namespace CompartmentalizedCreatureGraphics;

public static class CCG
{
    public readonly static CosmeticManager CosmeticManager = new CosmeticManager();
    public readonly static CosmeticEffectManager CosmeticEffectManager = new CosmeticEffectManager();

    /// <summary>
    /// Normalize the name to lowercase for consistency, so user's don't have to worry about case sensitivity when referencing cosmetics.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string ConvertTypeToCCGID(Type cosmeticType)
    {
        return cosmeticType.Name.ToLowerInvariant();
    }
}
