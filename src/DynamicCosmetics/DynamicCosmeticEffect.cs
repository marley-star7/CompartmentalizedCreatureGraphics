using UnityEngine;
using RWCustom;

namespace CompartmentalizedCreatureGraphics;

public class DynamicCosmeticEffect
{
    public DynamicCosmetic dynamicCosmetic;

    public virtual void OnCosmeticDrawSprites(RoomCamera.SpriteLeaser cosmeticWearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    public virtual void OnCosmeticUpdatePalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }
}
