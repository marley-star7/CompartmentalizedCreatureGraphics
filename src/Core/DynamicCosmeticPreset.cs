namespace CompartmentalizedCreatureGraphics.Core;

/// <summary>
/// Saved presets for the settings for a dynamic cosmetic.
/// </summary>
public class DynamicCosmeticPreset
{
    public DynamicCosmetic dynamicCosmetic;

    public string name;

    public DynamicCosmeticPreset(string name, DynamicCosmetic dynamicCosmetic)
    {
        this.name = name;
        this.dynamicCosmetic = dynamicCosmetic;
    }
}