// TODO: this will run after all dynamic cosmetic effects, and modify the effects in the list they are attached to.

namespace CompartmentalizedCreatureGraphics.Effects;

public class DynamicCosmeticEffect : UpdatableAndDeletable
{
    public class Properties : CCGCosmeticProperties
    {

    }

    protected Properties _properties;

    public Properties properties => properties;

    public IDynamicCreatureCosmetic cosmetic;
    public byte spriteEffectGroup = 0;

    public DynamicCosmeticEffect(IDynamicCreatureCosmetic cosmetic, byte spriteEffectGroup)
    {

    }

    public virtual void OnCosmeticInitiateSprites(RoomCamera.SpriteLeaser cosmeticSLeaser, RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    public virtual void OnCosmeticDrawSprites(RoomCamera.SpriteLeaser cosmeticSLeaser, RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    public virtual void OnCosmeticUpdatePalette(RoomCamera.SpriteLeaser cosmeticSLeaser, RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }
}
