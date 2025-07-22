using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompartmentalizedCreatureGraphics.Core;
using CompartmentalizedCreatureGraphics.Extensions;
using CompartmentalizedCreatureGraphics.SlugcatCosmetics;

namespace CompartmentalizedCreatureGraphics;

public static class Presets
{
    public static readonly float DefaultVanillaFaceSnapValue = 20f; //-- MR7: Found this is a good number to prevent weird angles, this or 10.

    internal static void AddPresets()
    {
        /*
        Content.AddDynamicCosmeticPreset(new DynamicCosmeticPreset(
            "Dynamic Classic Slugcat Left Ear",
            new DynamicSlugcatEarCosmetic()
            {
                spriteName = "ccgSlugcatEar",
                defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaLeftEarAnglePositions,
                side = -1,
                defaultScaleX = 1,
            }
        ));

        Content.AddDynamicCosmeticPreset(new DynamicCosmeticPreset(
            "Dynamic Classic Slugcat Right Ear",
            new DynamicSlugcatEarCosmetic()
            {
                spriteName = "ccgSlugcatEar",
                defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaRightEarAnglePositions,
                side = 1,
                defaultScaleX = -1,
            }
        ));

        Content.AddDynamicCosmeticPreset(new DynamicCosmeticPreset(
            "Classic Slugcat Left Eye",
            new DynamicSlugcatEyeCosmetic()
            {
                spriteName = "ccgSlugcatEye",
                defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaLeftEyeAnglePositions,
                side = -1,
                defaultScaleX = -1,
                snapValue = DefaultVanillaFaceSnapValue,
            }
        ));

        Content.AddDynamicCosmeticPreset(new DynamicCosmeticPreset(
            "Classic Slugcat Right Eye",
            new DynamicSlugcatEyeCosmetic()
            {
                spriteName = "ccgSlugcatEye",
                defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaRightEyeAnglePositions,
                side = 1,
                defaultScaleX = 1,
                snapValue = DefaultVanillaFaceSnapValue,
            }
        ));

        Content.AddDynamicCosmeticPreset(new DynamicCosmeticPreset(
            "Classic Slugcat Nose",
            new DynamicSlugcatFaceCosmetic()
            {
                spriteName = "ccgSlugcatNose",
                defaultAnglePositions = PlayerGraphicsCCGData.DefaultVanillaNoseAnglePositions,
                defaultScaleX = 1,
                snapValue = DefaultVanillaFaceSnapValue,
            }
        ));
        */
    }
}
