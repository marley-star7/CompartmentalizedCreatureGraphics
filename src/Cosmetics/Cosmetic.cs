using CompartmentalizedCreatureGraphics.Cosmetics;
using IL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics;

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

    protected Dictionary<int, SpriteLayer> _spriteLayers;
    public Dictionary<int, SpriteLayer> SpriteLayers {
        get => _spriteLayers; 
        set { _spriteLayers = value;  }
    }

    protected int startSpriteIndex;
    // TODO: fill this out so that it has the first sprite index refrence, and size. and would work with normal player sprites.

    public Cosmetic(Creature wearer, Dictionary<int, SpriteLayer>    spritesLayers)
    {
        this.wearer = wearer;
        this._spriteLayers = spritesLayers;
    }

    public void Equip(Creature wearer)
    {
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Add(this);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        foreach (var layer in SpriteLayers.Keys)
            wearerCCGData.layersCosmetics[layer].Add(this);
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
