namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// DynamicCosmetics are cosmetics able to be equipped and unequipped on demand.
/// </summary>
public class DynamicCreatureCosmetic : UpdatableAndDeletable, IDynamicCreatureCosmetic, IDrawable
{
    public class Properties
    {
        [JsonProperty("scaleX")]
        public float scaleX = 1f;

        [JsonProperty("scaleY")]
        public float scaleY = 1f;

        [JsonProperty("spriteLayerGroups")]
        public SpriteLayerGroup[] spriteLayerGroups = new SpriteLayerGroup[0]{};
    }

    public Creature wearer => _wearerGraphics.owner as Creature;

    protected GraphicsModule _wearerGraphics;
    public GraphicsModule wearerGraphics => _wearerGraphics;

    protected Properties _properties;

    protected RoomCamera.SpriteLeaser? _sLeaser;
    public RoomCamera.SpriteLeaser? sLeaser
    {
        get => _sLeaser;
    }

    public SpriteLayerGroup[] spriteLayerGroups
    {
        get => _properties.spriteLayerGroups;
    }
    public SpriteEffectGroup[] spriteEffectGroups;

    public DynamicCreatureCosmetic(GraphicsModule wearerGraphics, Properties properties)
    {
        this._wearerGraphics = wearerGraphics;
        this._properties = properties;
        this.spriteEffectGroups = new SpriteEffectGroup[0];

        wearerGraphics.AddCreatureCosmetic(this);
    }

    ~DynamicCreatureCosmetic()
    {
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Remove(this);
        // Properly remove this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < spriteLayerGroups.Length; i++)
            wearerCCGData.layersCosmetics[spriteLayerGroups[i].layer].Remove(this);

        this.Destroy();
    }

    //
    // WEARER IDRAWABLES
    //

    public virtual void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    //-- MR7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    public virtual void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }

    //
    // COLLISION
    //

    public virtual void OnWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {

    }

    public virtual void OnWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {

    }

    //
    // BASICALLY UNUSED IN FAVOR OF ONWEARER
    //

    public virtual void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        this._sLeaser = sLeaser;
    }

    public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        this._sLeaser = sLeaser;

        if (slatedForDeletetion || room != rCam.room)
        {
            sLeaser.CleanSpritesAndRemove();
        }
    }

    public virtual void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        this._sLeaser = sLeaser;
    }

    public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        this._sLeaser = sLeaser;
    }

    public void Unequip()
    {
        throw new NotImplementedException();
    }
}