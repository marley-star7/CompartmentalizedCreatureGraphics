namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Cosmetic used to store information about original sprites of a creature's graphics module.
/// So that it may be refrenced to by other cosmetics for sprite layering.
/// </summary>
public class OriginalCreatureGraphicsCosmeticReference : ICreatureCosmetic
{
    public GraphicsModule wearerGraphics;

    public RoomCamera.SpriteLeaser sLeaser
    {
        get => wearerGraphics.GetGraphicsModuleCCGData().sLeaser;
    }

    protected SpriteLayerGroup[] _spriteLayerGroups;
    public SpriteLayerGroup[] spriteLayerGroups {
        get => _spriteLayerGroups; 
        set { _spriteLayerGroups = value;  }
    }

    protected int startSpriteIndex;
    // TODO: fill this out so that it has the first sprite index refrence, and size. and would work with normal player sprites.

    public OriginalCreatureGraphicsCosmeticReference(GraphicsModule wearerGraphics, SpriteLayerGroup[] spriteLayerGroups)
    {
        this.wearerGraphics = wearerGraphics;
        this._spriteLayerGroups = spriteLayerGroups;

        wearerGraphics.AddCreatureCosmetic(this);
    }

    public void PostWearerInitiateSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam)
    {

    }

    public void PostWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }

    public void PostWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {

    }

    public void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    public void PostWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {

    }
}
