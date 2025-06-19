using UnityEngine;
using RWCustom;

namespace CompartmentalizedCreatureGraphics;

public class DynamicCosmetic : UpdatableAndDeletable, IDrawable
{
    public RoomCamera.SpriteLeaser sLeaser;

    public FSprite LastSprite
    {
        get => sLeaser.sprites[sLeaser.sprites.Length - 1];
    }

    public Creature wearer;

    public virtual void Equip(Creature wearer)
    {
        wearer.graphicsModule.GetGraphicsModuleCCGData().dynamicCosmetics.Add(this);
        this.wearer = wearer;

        if (this.wearer.room != null)
            wearer.room.AddObject(this);
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
        this.sLeaser = sLeaser;
    }

    public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        this.sLeaser = sLeaser;
    }

    public virtual void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {

    }

    public virtual void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
    {

    }
}