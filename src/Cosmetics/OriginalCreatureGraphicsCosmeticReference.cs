namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Cosmetic used to store information about original sprites of a creature's graphics module.
/// So that it may be refrenced to by other cosmetics for sprite layering.
/// </summary>
public class OriginalCreatureGraphicsCosmeticReference : ICreatureCosmetic
{
    public Creature wearer;

    public RoomCamera.SpriteLeaser SLeaser
    {
        get => wearer.graphicsModule.GetGraphicsModuleCCGData().sLeaser;
    }

    protected SpriteLayerGroup[] _spriteLayerGroups;
    public SpriteLayerGroup[] SpriteLayerGroups {
        get => _spriteLayerGroups; 
        set { _spriteLayerGroups = value;  }
    }

    protected int startSpriteIndex;
    // TODO: fill this out so that it has the first sprite index refrence, and size. and would work with normal player sprites.

    public OriginalCreatureGraphicsCosmeticReference(Creature wearer, SpriteLayerGroup[] spriteLayerGroups)
    {
        this.wearer = wearer;
        this._spriteLayerGroups = spriteLayerGroups;
    }

    public void Equip(Creature wearer)
    {
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Add(this);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < SpriteLayerGroups.Length; i++)
        {
            Plugin.LogDebug($"Adding cosmetic with sprite {this.SLeaser.sprites[SpriteLayerGroups[i].startSpriteIndex].element.name} to layer {SpriteLayerGroups[i].layer}");
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].layer].Add(this);
        }
    }

    public void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }

    public void OnWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {

    }

    public void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    public void OnWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {

    }
}
