using UnityEngine;
using RWCustom;

namespace CompartmentalizedCreatureGraphics;

public class Cosmetic : UpdatableAndDeletable, IDrawable
{
    public RoomCamera.SpriteLeaser sLeaser;

    public Creature wearer;

    public virtual void Equip(Creature wearer)
    {
        wearer.graphicsModule.GetGraphicsModuleCraftingData().cosmetics.Add(this);
        this.wearer = wearer;

        if (this.wearer.room != null)
            wearer.room.AddObject(this);
    }

    public virtual void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        this.sLeaser = sLeaser;
    }

    public virtual void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

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