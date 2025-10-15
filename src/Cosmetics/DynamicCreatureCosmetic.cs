namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// DynamicCosmetics are cosmetics able to be equipped and unequipped on demand.
/// </summary>
public class DynamicCreatureCosmetic : IDynamicCreatureCosmetic, IDrawable
{
    public abstract class Properties : CCGCosmeticProperties
    {
        [JsonProperty("scaleX")]
        public float scaleX = 1f;

        [JsonProperty("scaleY")]
        public float scaleY = 1f;

        // TODO: should probably seperate this to be changable in the presets instead of requiring creating whole new cosmetic properties just to change layer.
        /// <summary>
        /// The sprite layer groups this cosmetic uses.
        /// </summary>
        public SpriteLayerGroup[] spriteLayerGroups = new SpriteLayerGroup[0];

        /// <summary>
        /// The sprite effect groups this cosmetic uses.
        /// </summary>
        public SpriteEffectGroup[] spriteEffectGroups = new SpriteEffectGroup[0];

        /// <summary>
        /// Use protected virtual to allow inheriting classes to override the JsonProperty attribute if needed.
        /// </summary>
        protected abstract SpriteLayerGroup[] SpriteLayerGroupsSetter
        {
            set;
        }

        /// <summary>
        /// Use protected virtual to allow inheriting classes to override the JsonProperty attribute if needed.
        /// </summary>
        protected virtual SpriteEffectGroup[] SpriteEffectGroupsSetter
        {
            set => spriteEffectGroups = value;
        }

        public abstract DynamicCreatureCosmetic.Properties Parse(string json);
    }

    public Creature Wearer => _wearerGraphics.owner as Creature;

    protected GraphicsModule _wearerGraphics;
    public GraphicsModule WearerGraphics => _wearerGraphics;

    protected Properties _properties;

    protected RoomCamera.SpriteLeaser? _sLeaser;
    public RoomCamera.SpriteLeaser? SLeaser
    {
        get => _sLeaser;
    }

    public SpriteLayerGroup[] SpriteLayerGroups => _properties.spriteLayerGroups;

    public SpriteEffectGroup[] spriteEffectGroups;

    public SpriteEffectGroup[] SpriteEffectGroups => _properties.spriteEffectGroups;

    public DynamicCreatureCosmetic(GraphicsModule wearerGraphics, Properties properties)
    {
        this._wearerGraphics = wearerGraphics;
        this._properties = properties;
        this.spriteEffectGroups = new SpriteEffectGroup[0];
    }

    ~DynamicCreatureCosmetic()
    {
        var wearerCCGData = Wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Remove(this);
        // Properly remove this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < SpriteLayerGroups.Length; i++)
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].Layer].Remove(this);
    }

    //
    // WEARER IDRAWABLES
    //

    public virtual void PostWearerInitiateSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam)
    {

    }

    public virtual void PostWearerUpdate()
    {

    }

    public virtual void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    //-- MS7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    public virtual void PostWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }

    //
    // COLLISION
    //

    public virtual void PostWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {

    }

    public virtual void PostWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
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
    }

    public virtual void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        this._sLeaser = sLeaser;
    }

    public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        this._sLeaser = sLeaser;
    }
}