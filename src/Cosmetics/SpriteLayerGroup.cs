namespace CompartmentalizedCreatureGraphics.Cosmetics;

public struct SpriteLayerGroup
{
    [JsonProperty("layerName")]
    public int layer;
    
    /// <summary>
    /// The index of the start sprite for layer group
    /// </summary>
    [JsonProperty("startSpriteIndex")]
    public int startSpriteIndex;

    /// <summary>
    /// The index of the last sprite for layer group,
    /// </summary>
    [JsonProperty("endSpriteIndex")]
    public int endSpriteIndex;

    /// <summary>
    /// Wether this layer group is marked for re-ording.
    /// </summary>
    public bool needsReorder = false;

    /// <summary>
    /// Constructs a SpriteLayerGroup with only a single sprite in it.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="spriteIndex"></param>
    public SpriteLayerGroup(int layer, int spriteIndex)
    {
        this.layer = layer;
        this.startSpriteIndex = spriteIndex;
        endSpriteIndex = startSpriteIndex;
    }

    /// <summary>
    /// Constructs a layer group with multiple sprites in it.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="startSpriteIndex"></param>
    /// <param name="lastSpriteIndex"></param>
    public SpriteLayerGroup(int layer, int startSpriteIndex, int lastSpriteIndex)
    {
        this.layer = layer;
        this.startSpriteIndex = startSpriteIndex;
        this.endSpriteIndex = lastSpriteIndex;
    }
}
