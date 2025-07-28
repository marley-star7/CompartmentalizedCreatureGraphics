namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// DynamicCosmetics are cosmetics able to be equipped and unequipped on demand.
/// </summary>
public class DynamicCreatureCosmetic : UpdatableAndDeletable, IDynamicCreatureCosmetic, IDrawable
{
    public class Properties
    {
        public float scaleX = 1f;
        public float scaleY = 1f;

        public SpriteLayerGroup[] spriteLayerGroups = new SpriteLayerGroup[0];
    }

    private Creature _wearer;
    public Creature Wearer => _wearer;

    public GraphicsModule WearerGraphics => _wearer.graphicsModule;

    protected RoomCamera.SpriteLeaser? _sLeaser;
    public RoomCamera.SpriteLeaser? SLeaser
    {
        get => _sLeaser;
    }

    public SpriteLayerGroup[] _spriteLayerGroups;
    public SpriteLayerGroup[] SpriteLayerGroups
    {
        get => _spriteLayerGroups;
        set
        {
            _spriteLayerGroups = value;
        }
    }
    public SpriteEffectGroup[] spriteEffectGroups;

    public DynamicCreatureCosmetic(Creature wearer, SpriteLayerGroup[] spriteLayers)
    {
        this._wearer = wearer;
        this._spriteLayerGroups = spriteLayers;
        this.spriteEffectGroups = new SpriteEffectGroup[0];

        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Add(this);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < SpriteLayerGroups.Length; i++)
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].layer].Add(this);

        if (this._wearer.room != null)
            wearer.room.AddObject(this);
    }

    ~DynamicCreatureCosmetic()
    {
        var wearerCCGData = Wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Remove(this);
        // Properly remove this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < SpriteLayerGroups.Length; i++)
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].layer].Remove(this);

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
    }

    public virtual void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {

    }

    public virtual void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        newContainer ??= rCam.ReturnFContainer("Midground");

        foreach (FSprite fsprite in sLeaser.sprites)
        {
            fsprite.RemoveFromContainer();
            newContainer.AddChild(fsprite);
        }

        if (Wearer == null)
        {
            Plugin.LogError("Cannot add cosmetic to container - wearer is null");
            return;
        }
    }

    public void Unequip()
    {
        throw new NotImplementedException();
    }
}