namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class SlugcatCosmeticsPreset
{
    [JsonProperty("name")]
    public string name;

    [JsonProperty("dynamicCosmetics")]
    public List<CosmeticPreset> dynamicCosmetics = new();

    [JsonProperty("baseHeadSpriteName")]
    public string baseHeadSpriteName = "marError64";

    [JsonProperty("baseFaceSpriteName")]
    public string baseFaceSpriteName = "marError64";

    [JsonProperty("baseArmSpriteName")]
    public string baseArmSpriteName = "marError64";

    [JsonProperty("baseBodySpriteName")]
    public string baseBodySpriteName = "marError64";

    [JsonProperty("baseLegsSpriteName")]
    public string baseLegsSpriteName = "marError64";

    [JsonProperty("baseHipsSpriteName")]
    public string baseHipsSpriteName = "marError64";

    [JsonProperty("baseTailSpriteName")]
    public string baseTailSpriteName = "marError64";

    [JsonProperty("basePixelSpriteName")]
    public string basePixelSpriteName = "marError64";

    public SlugcatCosmeticsPreset(params CosmeticPreset[] dynamicCosmetics)
    {
        this.dynamicCosmetics.AddRange(dynamicCosmetics);
    }
}
