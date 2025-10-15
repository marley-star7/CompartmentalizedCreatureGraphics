namespace CompartmentalizedCreatureGraphics.Cosmetics;

public struct SpriteLayerGroup
{   
    // Ms7: the getters and setters are in ints for ease of use,
    // But internally they are stored as much smaller values for optimal struct packing.

    private ushort startSpriteIndex;

    /// <summary>
    /// The index of the start sprite for layer group
    /// </summary>
    [JsonProperty("startSpriteIndex")]
    public int StartSpriteIndex
    {
        get { return (int)startSpriteIndex; }
        set { startSpriteIndex = (byte)value; }
    }

    private ushort endSpriteIndex;

    /// <summary>
    /// The index of the last sprite for layer group,
    /// </summary>
    [JsonProperty("endSpriteIndex")]
    public int EndSpriteIndex
    {
        get { return (int)endSpriteIndex; }
        set { endSpriteIndex = (byte)value; }
    }

    private byte layer;

    [JsonProperty("layerName")]
    public int Layer
    {
        get { return (int)layer; }
        set { layer = (byte)value; }
    }

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
        this.Layer = layer;
        this.StartSpriteIndex = spriteIndex;
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
        this.Layer = layer;
        this.StartSpriteIndex = startSpriteIndex;
        this.EndSpriteIndex = lastSpriteIndex;
    }
}
