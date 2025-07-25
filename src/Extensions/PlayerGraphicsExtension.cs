﻿namespace CompartmentalizedCreatureGraphics.Extensions;

public class PlayerGraphicsCCGData : GraphicsModuleCCGData
{
    public SlugcatCosmeticsPreset cosmeticsPreset;

    //
    // DEFAULT CCG PLACEMENT VALUES FOR BUILDING SCUG.
    //

    //-- MR7: 1 Value = 1 pixel, it is all pixel perfect.

    public static readonly Vector2[] DefaultVanillaLeftEarAnglePositions = new Vector2[]
    {
        new Vector2(-4, 4),
    };

    public static readonly Vector2[] DefaultVanillaRightEarAnglePositions = new Vector2[]
    {
        new Vector2(4, 4),
    };

    //--MR7: 17 by 17, fix the it.
    public static readonly Vector2 DefaultVanillaLeftEyePosition = new Vector2(-4f, 3f);
    public static readonly Vector2[] DefaultVanillaLeftEyeAnglePositions = new Vector2[]
    {
        // Right commented values are what is added to the center ( default )
        DefaultVanillaLeftEyePosition + new Vector2(-3f, -2),
        DefaultVanillaLeftEyePosition + new Vector2(-2, -1),
        DefaultVanillaLeftEyePosition,
        DefaultVanillaLeftEyePosition + new Vector2(2, -1),
        DefaultVanillaLeftEyePosition + new Vector2(5f, -2)
    };

    public static readonly Vector2 DefaultVanillaRightEyePosition = new Vector2(4f, 3f);
    public static readonly Vector2[] DefaultVanillaRightEyeAnglePositions = new Vector2[]
    {
        // Right commented values are what is added to the center ( default )
        DefaultVanillaRightEyePosition + new Vector2(-5f, -2),
        DefaultVanillaRightEyePosition + new Vector2(-2, -1),
        DefaultVanillaRightEyePosition,
        DefaultVanillaRightEyePosition + new Vector2(2, -1),
        DefaultVanillaRightEyePosition + new Vector2(3f, -2)
    };

    public static readonly Vector2[] DefaultVanillaNoseAnglePositions = new Vector2[]
    {
        new Vector2(-6f, -2),
        new Vector2(-3, -1),
        new Vector2(0, 0),
        new Vector2(3, -1),
        new Vector2(6f, -2),
    };

    // The Magic Sheet of Sprite Bull-Sheet
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

    public FSprite? BaseBodySprite
    {
        get { return sLeaser.sprites[0]; }
    }
    public FSprite? BaseHipsSprite
    {
        get { return sLeaser.sprites[1]; }
    }
    public FSprite? BaseTailSprite
    {
        get { return sLeaser.sprites[2]; }
    }
    public FSprite? BaseHeadSprite
    {
        get { return sLeaser.sprites[3]; }
    }
    public FSprite? BaseLegsSprite
    {
        get { return sLeaser.sprites[4]; }
    }
    public FSprite? BaseLeftArmSprite
    {
        get { return sLeaser.sprites[5]; }
    }
    public FSprite? BaseRightArmSprite
    {
        get { return sLeaser.sprites[6]; }
    }
    public FSprite? BaseLeftTerrainHandSprite
    {
        get { return sLeaser.sprites[7]; }
    }
    public FSprite? BaseRightTerrainHandSprite
    {
        get { return sLeaser.sprites[8]; }
    }
    public FSprite? BaseFaceSprite
    {
        get { return sLeaser.sprites[9]; }
    }
    public FSprite? BasePixelSprite
    {
        get { return sLeaser.sprites[11]; }
    }

    public int faceAngleNum = 0;
    public int faceSide
    {
        get { return Math.Sign(faceAngleNum); }
    }
    public string faceAngle = "A0";
    public string faceAngleAsymmetrical = "A0";

    public WeakReference<PlayerGraphics> playerGraphicsRef;

    public PlayerGraphicsCCGData(PlayerGraphics playerGraphicsRef) : base (playerGraphicsRef)
    {
        this.playerGraphicsRef = new WeakReference<PlayerGraphics>(playerGraphicsRef);

        // Construct the cosmeticLayers dictionary depending on the size of the amount of layers in the enum.
        layersCosmetics = new Dictionary<int, List<ICosmetic>>();

        var enumSize = Enum.GetValues(typeof(CCGEnums.SlugcatCosmeticLayer)).Length;
        for (int i = 0; i < enumSize; i++)
        {
            layersCosmetics.Add(i, new List<ICosmetic>());
        }
    }

    public Vector2 facePos = Vector2.zero;
    public float faceRotation = 0;
    public float snappedFaceRotationSnapDegrees = 0;
}

public static class PlayerGraphicsCCGExtension
{
    /// <summary>
    /// Dir char set to R for right, or L for left, leave empty for nothing.
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <param name="angleNum"></param>
    /// <param name="dirChar"></param>
    public static void SetFaceAngle(this PlayerGraphics playerGraphics, int angleNum)
    {
        var ccgData = playerGraphics.GetPlayerGraphicsCCGData();
        ccgData.faceAngleNum = angleNum;

        switch (angleNum)
        {
            case -2:
                ccgData.faceAngle = "A2";
                ccgData.faceAngleAsymmetrical = "-A2";
                break;
            case -1:
                ccgData.faceAngle = "A1";
                ccgData.faceAngleAsymmetrical = "-A1";
                break;
            case 0:
                ccgData.faceAngle = "A0";
                ccgData.faceAngleAsymmetrical = "A0";
                break;
            case 1:
                ccgData.faceAngle = "A1";
                ccgData.faceAngleAsymmetrical = "+A1";
                break;
            case 2:
                ccgData.faceAngle = "A2";
                ccgData.faceAngleAsymmetrical = "+A2";
                break;
        }
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

    public static void EquipSlugcatCosmeticsPreset(this PlayerGraphics playerGraphics, SlugcatCosmeticsPreset preset)
    {
        var ccgData = playerGraphics.GetPlayerGraphicsCCGData();

        ccgData.cosmeticsPreset = preset;
        foreach (var cosmetic in ccgData.cosmeticsPreset.dynamicCosmetics)
        {
            cosmetic.Equip(playerGraphics.player);
        }

        ccgData.compartmentalizedGraphicsEnabled = true;
    }

    public static void EquipCosmetic(this GraphicsModule graphicsModule, ICosmetic cosmetic)
    {
        cosmetic.Equip((Creature)graphicsModule.owner);
    }

    /// <summary>
    /// Creates a cosmetic that simply holds the information of the base player graphics sprites in the cosmetics system.
    /// This is so that we can place cosmetics in front and behind the base player graphics sprites, and also so that we can easily access the base player graphics sprites in the cosmetics system.
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <returns></returns>
    internal static Cosmetic CreateBasePlayerGraphicsReferenceCosmetic(this PlayerGraphics playerGraphics)
    {
        // The Magic Sheet of Sprite Bull-Sheet
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

        return new Cosmetic((Player)playerGraphics.owner,
            new SpriteLayerGroup[]
            {
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseBody, 0),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseHips, 1),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseTail, 2),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseHead, 3),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseLegs, 4),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseLeftArm, 5),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseRightArm, 6),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseLeftTerrainHand, 7),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseRightTerrainHand, 8),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseFace, 9),
            }
        );
    }

    public static PlayerGraphicsCCGData GetPlayerGraphicsCCGData(this PlayerGraphics playerGraphics) => (PlayerGraphicsCCGData) GraphicsModuleCraftingExtension.craftingDataConditionalWeakTable.GetValue(playerGraphics, _ => new PlayerGraphicsCCGData(playerGraphics));
}