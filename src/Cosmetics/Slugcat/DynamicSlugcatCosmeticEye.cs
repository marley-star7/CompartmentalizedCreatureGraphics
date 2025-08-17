namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticEye : DynamicSlugcatFaceCosmetic
{
    public DynamicSlugcatCosmeticEye(PlayerGraphics wearerGraphics, Properties properties) : base(wearerGraphics, properties)
    {
    }

    public override void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (sLeaser == null)
            return;

        base.PostWearerDrawSprites(wearerSLeaser, rCam, timeStacker, camPos);
        //-- MS7: TODO: Maybe make the sideways head turn sprite move two pixels down only when sideways.

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        var extraText = "";

        if (playerGraphics.blink > 0)
            extraText = "Blink";
        else if (playerGraphics.player.Stunned)
            extraText = "Stunned";
        else if (playerGraphics.player.dead)
            extraText = "Dead";

        if (playerGraphicsCCGData.faceSide == properties.side)
            extraText += playerGraphicsCCGData.faceSpriteAngle;
        else
            extraText += "A0";

        for (int i = 0; i < properties.spriteNames.Length; i++)
        {
            sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName(properties.spriteNames[i] + extraText);
        }
    }
}
