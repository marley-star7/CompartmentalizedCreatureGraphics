namespace CompartmentalizedCreatureGraphics.Cosmetics;

public struct SpriteLayerGroup
{
    [JsonProperty("layerName")]
    public int layer;

    [JsonProperty("startSpriteIndex")]
    public int startSpriteIndex;

    [JsonProperty("endSpriteIndex")]
    public int endSpriteIndex;

    public bool needsReorder = false;

    public SpriteLayerGroup(int layer, int firstSpriteIndex)
    {
        this.layer = layer;
        this.startSpriteIndex = firstSpriteIndex;
        endSpriteIndex = firstSpriteIndex;
    }

    public SpriteLayerGroup(int layer, int firstSpriteIndex, int lastSpriteIndex)
    {
        this.layer = layer;
        this.startSpriteIndex = firstSpriteIndex;
        this.endSpriteIndex = lastSpriteIndex;
    }
}
