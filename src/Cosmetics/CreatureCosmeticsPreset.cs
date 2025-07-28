namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Saved presets for the settings for a dynamic cosmetic.
/// </summary>
public class CreatureCosmeticsPreset
{
    public IDynamicCreatureCosmetic dynamicCosmetic;

    public string name;

    public CreatureCosmeticsPreset(string name, IDynamicCreatureCosmetic dynamicCosmetic)
    {
        this.name = name;
        this.dynamicCosmetic = dynamicCosmetic;
    }
}