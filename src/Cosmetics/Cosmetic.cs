namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Cosmetics unlike DynamicCosmetics cannot be unequipped.
/// They are usually used to store information about original sprites of a creature's graphics module.
/// </summary>
public class Cosmetic : ICosmetic
{
    public Creature wearer;

    public RoomCamera.SpriteLeaser SpriteLeaser
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

    public Cosmetic(Creature wearer, SpriteLayerGroup[] spriteLayerGroups)
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
            Plugin.DebugLog($"Adding cosmetic with sprite {this.SpriteLeaser.sprites[SpriteLayerGroups[i].firstSpriteIndex].element.name} to layer {SpriteLayerGroups[i].layer}");
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].layer].Add(this);
        }
    }

    public FSprite FirstSprite
    {
        get => SpriteLeaser.sprites[startSpriteIndex];
    }

    public FSprite LastSprite
    {
        get => SpriteLeaser.sprites[startSpriteIndex];
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
