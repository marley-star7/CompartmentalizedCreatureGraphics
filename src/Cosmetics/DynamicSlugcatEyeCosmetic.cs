using UnityEngine;
using RWCustom;

using MRCustom.Math;
using CompartmentalizedCreatureGraphics.Extensions;

namespace CompartmentalizedCreatureGraphics.SlugcatCosmetics;

public class DynamicSlugcatEyeCosmetic : DynamicSlugcatFaceCosmetic
{
    public DynamicSlugcatEyeCosmetic(Dictionary<int, SpriteLayer> spriteLayers) : base(spriteLayers)
    {
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        base.OnWearerDrawSprites(wearerSLeaser, rCam, timeStacker, camPos);
        //-- MR7: TODO: Maybe make the sideways head turn sprite move two pixels down only when sideways.

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        var extraText = "";

        if (playerGraphics.blink > 0)
            extraText = "Blink";
        else if (playerGraphics.player.Stunned)
            extraText = "Stunned";
        else if (playerGraphics.player.dead)
            extraText = "Dead";

        if (playerGraphicsCCGData.faceSide == side)
            Sprite.element = Futile.atlasManager.GetElementWithName(spriteName + extraText + playerGraphicsCCGData.faceAngle);
        else
            Sprite.element = Futile.atlasManager.GetElementWithName(spriteName + extraText + "A0");

        //-- MR7: Snap the rotation and placement so it doesn't break at weird spots.
        Sprite.rotation = MarMathf.Snap(playerGraphicsCCGData.faceRotation, snapValue);
    }
}
