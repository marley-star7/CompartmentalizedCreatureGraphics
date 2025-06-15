using RWCustom;
using UnityEngine;

using MRCustom.Animations;
using MRCustom.Math;

namespace CompartmentalizedCreatureGraphics;

public static partial class Hooks
{
    //-- Add hooks
    internal static void ApplyPlayerGraphicsHooks()
    {
        On.PlayerGraphics.Update += PlayerGraphics_Update;

        On.PlayerGraphics.InitiateSprites += PlayerGraphics_InitiateSprites;
        On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        On.PlayerGraphics.ApplyPalette += PlayerGraphics_ApplyPalette;
    }

    //-- Remove hooks
    internal static void RemovePlayerGraphicsHooks()
    {
        On.PlayerGraphics.Update -= PlayerGraphics_Update;

        On.PlayerGraphics.InitiateSprites -= PlayerGraphics_InitiateSprites;
        On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        On.PlayerGraphics.ApplyPalette -= PlayerGraphics_ApplyPalette;
    }

    /* 
    Sprite 0 = BodyA
    Sprite 1 = HipsA
    Sprite 2 = Tail
    Sprite 3 = HeadA || B
    Sprite 4 = LegsA
    Sprite 5 = Arm
    Sprite 6 = Arm
    Sprite 7 = TerrainHand
    sprite 8 = TerrainHand
    sprite 9 = FaceA
    sprite 10 = Futile_White with shader Flatlight
    sprite 11 = pixel Mark of comunication
    */

    private static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);
    }

    //
    // IDRAWABLE
    //

    private static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        var playerGraphicsData = playerGraphics.GetPlayerGraphicsCraftingData();
        //-- MR7: TODO: might be able to remove this sLeaser storing, since cosmetics have direct refrence to it via the function calls...
        playerGraphicsData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam);

        if (!playerGraphicsData.dynamicSlugcatGraphicsEnabled)
            return;

        //
        // BUILD THE SCUG
        //

        //-- Hide the original face since we are using new one.
        // We do this instead of just "removing" sprites[9] for compatability purposes.
        var origFaceSprite = sLeaser.sprites[9];
        origFaceSprite.color = Color.black;

        var face = new SlugcatFace()
        {
            faceSpriteName = "crafter_Face"
        };
        playerGraphics.EquipCosmetic(face);

        var leftEar = new SlugcatEar()
        {
            defaultPosOffsetFromHead = new Vector2(-4, 4),
        };
        playerGraphics.EquipCosmetic(leftEar);

        var rightEar = new SlugcatEar()
        {
            defaultScaleX = -1,
            defaultPosOffsetFromHead = new Vector2(4, 4)
        };
        playerGraphics.EquipCosmetic(rightEar);

    }

    private static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var playerGraphicsData = playerGraphics.GetPlayerGraphicsCraftingData();
        orig(playerGraphics, sLeaser, rCam, timeStacker, camPos);

        var playerData = playerGraphics.player.GetPlayerCraftingData();

        if (!playerGraphics.player.IsCrafter())
            return;

        //-- TODO: when idle for a bit, he will close his eyes in peace.

        //
        // WORKING ON PLAYER GRAPHICAL REWORK STUFF
        //

        playerGraphicsData.HeadSprite.element = Futile.atlasManager.GetElementWithName("slugcatHead_Head");

        //
        // COSMETICS REQUIRED UPDATE THING
        //

        for (int i = 0; i < playerGraphicsData.cosmetics.Count; i++)
        {
            playerGraphicsData.cosmetics[i].OnWearerDrawSprites(sLeaser, rCam, timeStacker, camPos);
        }
    }

    private static void PlayerGraphics_ApplyPalette(On.PlayerGraphics.orig_ApplyPalette orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        orig(playerGraphics, sLeaser, rCam, palette);

        if (!playerGraphics.player.IsCrafter())
            return;

        //-- MR7: It's barely noticable, but basing the color off the room pallete makes it look a bit better.
        // There is also potential issue that comes with a gray scug that depending on the room palette, especially fog color, they can become almost impossible to see.
        // Tried making some code for this to find an optimal gray based both off the room palette, and fog color, for max readability.
        // (and to help the colorblind folks out)

        float idealLerpRatio = 0.6f;
        float visibilityLerpRatioModifierFullStrength = 0.13f;

        Color roomBlackColor = palette.GetColor(RoomPalette.ColorName.BlackColor);
        Color roomFogColor = palette.GetColor(RoomPalette.ColorName.FogColor);

        Color idealGray = Color.Lerp(roomBlackColor, Color.white, idealLerpRatio);

        float adjustmentIfTooCloseRatio;
        if (idealGray.grayscale < roomFogColor.grayscale)
        {
            //Plugin.Logger.LogDebug("Ideal gray is darker than room fog color gray, lightening the gray even further.");
            float inverseLerp = Mathf.InverseLerp(0, roomFogColor.grayscale, idealGray.grayscale);
            adjustmentIfTooCloseRatio = inverseLerp * visibilityLerpRatioModifierFullStrength * 1.5f; //- MR7: Multiply a bit more since we need to put in extra work to get out of the darker section.
            idealGray = Color.Lerp(roomBlackColor, Color.white, idealLerpRatio + adjustmentIfTooCloseRatio);
        }
        else
        {
            //Plugin.Logger.LogDebug("Ideal gray is lighter than room fog color gray, lighter the gray further.");
            float inverseLerp = Mathf.InverseLerp(roomFogColor.grayscale, Color.white.grayscale, idealGray.grayscale);
            adjustmentIfTooCloseRatio = (1 - inverseLerp) * visibilityLerpRatioModifierFullStrength; // 1 - 0 because the inverse lerp here is closest to ideal gray at 0
            idealGray = Color.Lerp(roomBlackColor, Color.white, idealLerpRatio + adjustmentIfTooCloseRatio);
        }

        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].color = idealGray;
        }
    }
}
