using MRCustom;
using System.Xml.Linq;

namespace CompartmentalizedCreatureGraphics.Extensions;

public class PlayerGraphicsCCGData : GraphicsModuleCCGData
{
    public SlugcatCosmeticsPreset cosmeticsPreset;

    //
    // DEFAULT CCG PLACEMENT VALUES FOR BUILDING SCUG.
    //

    //-- MS7: 1 Value = 1 pixel, it is all pixel perfect.

    public static readonly Vector2[] DefaultVanillaLeftEarAnglePositions = new Vector2[]
    {
        new Vector2(-4, 4),
    };

    public static readonly Vector2[] DefaultVanillaRightEarAnglePositions = new Vector2[]
    {
        new Vector2(4, 4),
    };

    //--MS7: 17 by 17, fix the it.
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
        get { return sLeaser.sprites[SpriteIndexes.Player.Body]; }
    }
    public FSprite? BaseHipsSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.Hips]; }
    }
    public FSprite? BaseTailSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.Tail]; }
    }
    public FSprite? BaseHeadSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.Head]; }
    }
    public FSprite? BaseLegsSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.Legs]; }
    }
    public FSprite? BaseLeftArmSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.LeftArm]; }
    }
    public FSprite? BaseRightArmSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.RightArm]; }
    }
    public FSprite? BaseLeftTerrainHandSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.LeftTerrainHand]; }
    }
    public FSprite? BaseRightTerrainHandSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.RightTerrainHand]; }
    }
    public FSprite? BaseFaceSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.Face]; }
    }
    public FSprite? BaseMarkSprite
    {
        get { return sLeaser.sprites[SpriteIndexes.Player.Mark]; }
    }

    public enum FaceSpriteAnglingMode
    {
        /// <summary>
        /// The face angle will not be changed by any automatic system.
        /// </summary>
        Manual,
        /// <summary>
        /// The face angle will lerp slowly according to headDepthRotation.
        /// </summary>
        LerpHeadDepthRotation,
        /// <summary>
        /// The face angle will rotate to match the direction the body would be facing,
        /// Looking sideways when the body is horiztonal, and straight when vertical.
        /// </summary>
        AngleWithBodyDirection,

        FullAngleToBodyDirection,
    }

    public FaceSpriteAnglingMode faceSpriteAnglingMode = FaceSpriteAnglingMode.LerpHeadDepthRotation;

    public int faceSpriteAngleNum = 0;
    public int faceSide
    {
        get { return Math.Sign(faceSpriteAngleNum); }
    }
    /// <summary>
    /// Face angle goes between (A0, A1, A2)
    /// </summary>
    public string faceSpriteAngle = "A0";
    /// Asymmetrical face angle goes between (-A2, -A1, A0, A1, A2)
    /// </summary>
    public string faceSpriteAngleAsymmetrical = "A0";

    public WeakReference<PlayerGraphics> playerGraphicsRef;

    public PlayerGraphicsCCGData(PlayerGraphics playerGraphicsRef) : base (playerGraphicsRef)
    {
        this.playerGraphicsRef = new WeakReference<PlayerGraphics>(playerGraphicsRef);

        // Construct the cosmeticLayers dictionary depending on the size of the amount of layers in the enum.
        layersCosmetics = new Dictionary<int, List<ICreatureCosmetic>>();

        var enumSize = Enum.GetValues(typeof(Enums.SlugcatCosmeticLayer)).Length;
        for (int i = 0; i < enumSize; i++)
        {
            layersCosmetics.Add(i, new List<ICreatureCosmetic>());
        }
    }

    public Vector2 facePos = Vector2.zero;

    public Vector2 faceRotation;
    public Vector2 lastFaceRotation;

    public float headDepthRotation;
    public float lastHeadDepthRotation;

    public float targetHeadDepthRotation;
}

public static class PlayerGraphicsCCGExtensions
{
    public static PlayerGraphicsCCGData GetPlayerGraphicsCCGData(this PlayerGraphics playerGraphics)
    {
        if (playerGraphics == null)
            throw new ArgumentNullException(nameof(playerGraphics));

        return (PlayerGraphicsCCGData)GraphicsModuleCCGExtensions.ccgDataConditionalWeakTable.GetValue(
            (GraphicsModule)playerGraphics,
            _ => new PlayerGraphicsCCGData(playerGraphics));
    }

    /// <summary>
    /// -2 = "-A2"
    /// -1 = "-A1"
    /// 0 = "-A0"
    /// 1 = "A1"
    /// 2 = "A2"
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <param name="angleNum"></param>
    public static void SetFaceSpriteAngle(this PlayerGraphics playerGraphics, int angleNum)
    {
        var ccgData = playerGraphics.GetPlayerGraphicsCCGData();

        ccgData.faceSpriteAngleNum = angleNum;

        var angle = GetFaceSpriteAngleAsymmetrical(angleNum);
        ccgData.faceSpriteAngleAsymmetrical = angle;
        ccgData.faceSpriteAngle = GraphicsModuleCCGExtensions.GetSymmetricalAngleFromAsymmetrical(angle);
    }

    public static void SetFaceSpriteAngle(this PlayerGraphics playerGraphics, string spriteAngle)
    {
        var ccgData = playerGraphics.GetPlayerGraphicsCCGData();

        ccgData.faceSpriteAngle = spriteAngle;
        ccgData.faceSpriteAngleAsymmetrical = spriteAngle;

        ccgData.faceSpriteAngleNum = GetFaceSpriteAngleNum(spriteAngle);
    }

    public static string GetFaceSpriteAngleAsymmetrical(int angleNum)
    {
        switch (angleNum)
        {
            case -2:
                return "-A2";
            case -1:
                return "-A1";
            case 1:
                return "A1";
            case 2:
                return "A2";
            default:
                return "A0";
        }
    }

    public static int GetFaceSpriteAngleNum(string faceSpriteAngle)
    {
        switch (faceSpriteAngle)
        {
            case "-A2":
                return -2;
            case "-A1":
                return -1;
            case "A1":
                return 1;
            case "A2":
                return 2;
            default:
                return 0;
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

    //
    //
    //

    /// <summary>
    /// Creates a cosmetic that simply holds the information of the base player graphics sprites in the cosmetics system.
    /// This is so that we can place cosmetics in front and behind the base player graphics sprites, and also so that we can easily access the base player graphics sprites in the cosmetics system.
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <returns></returns>
    internal static void CreateAndAddOriginalPlayerGraphicsCosmeticReference(this PlayerGraphics playerGraphics)
    {
        new OriginalCreatureGraphicsCosmeticReference(playerGraphics,
            new SpriteLayerGroup[]
            {
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.Back, SpriteIndexes.Player.Tail),

                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseTail, SpriteIndexes.Player.Tail),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseBody, SpriteIndexes.Player.Body),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseLegs, SpriteIndexes.Player.Legs),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseHips, SpriteIndexes.Player.Hips),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseHead, SpriteIndexes.Player.Head),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseFace, SpriteIndexes.Player.Face),

                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseLeftArm, SpriteIndexes.Player.LeftArm),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseRightArm, SpriteIndexes.Player.RightArm),

                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseLeftTerrainHand, SpriteIndexes.Player.LeftTerrainHand),
                new SpriteLayerGroup((int)Enums.SlugcatCosmeticLayer.BaseRightTerrainHand, SpriteIndexes.Player.RightTerrainHand),
            }
        );
    }

    public static void CreateAndAddDynamicSlugcatCosmetic(this PlayerGraphics self, string cosmeticTypeId, string propertiesId)
    {
        Plugin.LogDebug($"Equipping cosmeticTypeID {cosmeticTypeId} of propertiesID {propertiesId} to player");
        self.AddCreatureCosmetic(CCG.CosmeticManager.GetCritcosFromCosmeticTypeId(cosmeticTypeId).CreateDynamicCosmeticForCreature(self, propertiesId));
    }

    public static void CreateAndAddSlugcatCosmeticsPreset(this PlayerGraphics self, SlugcatCosmeticsPreset preset)
    {
        var ccgData = self.GetPlayerGraphicsCCGData();

        ccgData.cosmeticsPreset = preset;
        for (int i = 0; i < preset.dynamicCosmetics.Count; i++)
        {
            CreateAndAddDynamicSlugcatCosmetic(self, preset.dynamicCosmetics[i].cosmeticTypeId, preset.dynamicCosmetics[i].propertiesId);
        }

        ccgData.compartmentalizedGraphicsEnabled = true;
    }

    public static int GetFaceAngleForRotation(Vector2 rotation)
    {
        switch (rotation.x)
        {
            case > 0.9f:
                return 2;

            case > 0.45f:
                return 1;

            case < -0.9f:
                return -2;

            case < -0.45f:
                return -1;

            default:
                return 0;

        }
    }
}