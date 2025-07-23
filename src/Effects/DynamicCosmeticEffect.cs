// TODO: this will run after all dynamic cosmetic effects, and modify the effects in the list they are attached to.

namespace CompartmentalizedCreatureGraphics.Effects;

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
