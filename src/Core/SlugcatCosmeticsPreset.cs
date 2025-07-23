using CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics;

public class SlugcatCosmeticsPreset
{
    public static DynamicSlugcatEarCosmetic CreateDefaultVanillaSlugcatDynamicLeftEarCosmetic()
    {
        return new DynamicSlugcatEarCosmetic(new SpriteLayerGroup[]{ new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Ears, 0) })
        {
            spriteName = "ccgSlugcatEar",
            defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaLeftEarAnglePositions,
            earColor = new Color(1f, 1f, 1f, 1f), // MR7: Default color is white.
            rad = 5f, // MR7: Default radius of ear is 5 pixels.
            defaultScaleX = 1,
            side = -1
        };
    }

    public static DynamicSlugcatEarCosmetic CreateDefaultVanillaSlugcatDynamicRightEarCosmetic()
    {
        return new DynamicSlugcatEarCosmetic(new SpriteLayerGroup[] { new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Ears, 0) })
        {
            spriteName = "ccgSlugcatEar",
            defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaRightEarAnglePositions,
            earColor = new Color(1f, 1f, 1f, 1f), // MR7: Default color is white.
            rad = 5f, // MR7: Default radius of ear is 5 pixels.
            defaultScaleX = -1,
            side = 1
        };
    }

    public static DynamicSlugcatFaceCosmetic CreateDefaultVanillaSlugcatDynamicNoseCosmetic()
    {
        return new DynamicSlugcatFaceCosmetic(
            new SpriteLayerGroup[]{
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Nose, 0)
            })
        {
            spriteName = "ccgSlugcatNose",
            defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaNoseAnglePositions,
            defaultScaleX = 1,
            snapValue = 15, // MR7: Default snap value for nose is 15.
        };
    }

    public static DynamicSlugcatFaceCosmetic CreateDefaultVanillaSlugcatLeftEyeCosmetic()
    {
        return new DynamicSlugcatFaceCosmetic(
            new SpriteLayerGroup[]{
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Nose, 0)
            })
        {
            spriteName = "ccgSlugcatNose",
            defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaNoseAnglePositions,
            defaultScaleX = 1,
            snapValue = 15, // MR7: Default snap value for nose is 15.
        };
    }

    public static DynamicSlugcatFaceCosmetic CreateDefaultVanillaSlugcatRightEyeCosmetic()
    {
        return new DynamicSlugcatFaceCosmetic(
            new SpriteLayerGroup[]{
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Nose, 0)
            })
        {
            spriteName = "ccgSlugcatNose",
            defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaNoseAnglePositions,
            defaultScaleX = 1,
            snapValue = 15, // MR7: Default snap value for nose is 15.
        };
    }

    public List<DynamicCosmetic> dynamicCosmetics = new List<DynamicCosmetic>();

    public string baseHeadSpriteName = "marError64";
    public string baseFaceSpriteName = "marError64";
    public string baseBodySpriteName = "marError64";
    public string baseArmSpriteName = "marError64";
    public string baseLegsSpriteName = "marError64";
    public string baseHipsSpriteName = "marError64";
    public string baseTailSpriteName = "marError64";
    public string basePixelSpriteName = "marError64";

    public SlugcatCosmeticsPreset(params DynamicCosmetic[] dynamicCosmetics)
    {
        this.dynamicCosmetics.AddRange(dynamicCosmetics);
    }
}
