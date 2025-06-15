using UnityEngine;
using RWCustom;

using System.Runtime.CompilerServices;

namespace CompartmentalizedCreatureGraphics;

public class PlayerGraphicsCraftingData : GraphicsModuleCCGData
{
    // The Magic List of All Sprites
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

    public string FaceAngle = "A0";

    public FSprite HeadSprite
    {
        get { return sLeaser.sprites[3]; }
    }
    public FSprite OriginalFaceSprite
    {
        get { return sLeaser.sprites[9]; }
    }

    public SlugcatFace SlugcatFace;

    public WeakReference<PlayerGraphics> playerGraphicsRef;

    public PlayerGraphicsCraftingData(PlayerGraphics playerGraphicsRef) : base (playerGraphicsRef)
    {
        this.playerGraphicsRef = new WeakReference<PlayerGraphics>(playerGraphicsRef);
    }

    //
    // DATA FOR THE THINGY GRAPHICS SLUG THINGY
    //

    public bool dynamicSlugcatGraphicsEnabled = false;
    public float headFaceRotationDegrees = 0;
}

public static class PlayerGraphicsCraftingExtension
{
    public static void SetFaceAngle(this PlayerGraphics playerGraphics, int angleNum)
    {
        playerGraphics.GetPlayerGraphicsCraftingData().FaceAngle = ("A" + angleNum);
    }

    /// <summary>
    /// Returns the name of the face without any custom faces applied.
    /// Useful if just want the face angle, not checking if it is saint or crafter's face.
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <param name="faceSpriteName"></param>
    /// <returns></returns>
    public static string GetBaseFaceSpriteName(this PlayerGraphics playerGraphics, string faceSpriteName)
    {
        return "Face" + faceSpriteName[faceSpriteName.Length - 2] + faceSpriteName[faceSpriteName.Length - 1];
    }

    public static string GetNoEmotionFaceSpriteAngle(this PlayerGraphics playerGraphics, string faceSpriteName)
    {
        var angleName = faceSpriteName.Substring(faceSpriteName.Length - 2);
        angleName = faceSpriteName.Remove(faceSpriteName.Length - 2, 1);
        angleName = faceSpriteName.Insert(faceSpriteName.Length - 1, "A");

        // If does not A at this point, it is a stunned or dead face.
        if (!faceSpriteName.Contains('A'))
            faceSpriteName = "A0";

        return faceSpriteName;
    }


    /// <summary>
    /// Returns the name of the face sprite with correct data for angle without blinking and dead faces name.
    /// Useful for if only need the angle for a cosmetic.
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <param name="faceSpriteName"></param>
    /// <returns></returns>
    public static string GetNoEmotionFaceSpriteName(this PlayerGraphics playerGraphics, string faceSpriteName)
    {
        faceSpriteName = faceSpriteName.Remove(faceSpriteName.Length - 2, 1);
        faceSpriteName = faceSpriteName.Insert(faceSpriteName.Length - 1, "A");

        // If does not A at this point, it is a stunned or dead face.
        if (!faceSpriteName.Contains('A'))
            faceSpriteName = "FaceA0";

        return faceSpriteName;
    }

    private static readonly ConditionalWeakTable<PlayerGraphics, PlayerGraphicsCraftingData> craftingDataConditionalWeakTable = new();

    public static PlayerGraphicsCraftingData GetPlayerGraphicsCraftingData(this PlayerGraphics playerGraphics) => (PlayerGraphicsCraftingData) GraphicsModuleCraftingExtension.craftingDataConditionalWeakTable.GetValue(playerGraphics, _ => new PlayerGraphicsCraftingData(playerGraphics));
}