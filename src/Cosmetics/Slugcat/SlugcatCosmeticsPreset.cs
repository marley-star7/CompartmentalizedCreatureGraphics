namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class SlugcatCosmeticsPreset
{
    public List<(string, string)> dynamicCosmetics = new();

    public string baseHeadSpriteName = "marError64";
    public string baseFaceSpriteName = "marError64";
    public string baseBodySpriteName = "marError64";
    public string baseArmSpriteName = "marError64";
    public string baseLegsSpriteName = "marError64";
    public string baseHipsSpriteName = "marError64";
    public string baseTailSpriteName = "marError64";
    public string basePixelSpriteName = "marError64";

    public SlugcatCosmeticsPreset(params (string, string)[] dynamicCosmetics)
    {
        this.dynamicCosmetics.AddRange(dynamicCosmetics);
    }

    /*
    public static (Type, DynamicSlugcatEarCosmeticSprite.Properties) VanillaSlugcatLeftEarCosmetic()
    {
        return (typeof(DynamicSlugcatEarCosmeticSprite), new DynamicSlugcatEarCosmeticSprite.Properties(new SpriteLayerGroup[]
            { new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Ears, 0) })
        {
            spriteName = "ccgSlugcatEar",
            anglePositions = PlayerGraphicsCCGData.DefaultVanillaLeftEarAnglePositions,
            earColor = new Color(1f, 1f, 1f, 1f), // MR7: Default color is white.
            rad = 5f, // MR7: Default radius of ear is 5 pixels.
            scaleX = 1,
            side = -1
        });
    }

    public static (Type, DynamicSlugcatEarCosmeticSprite.Properties) VanillaSlugcatRightEarCosmetic()
    {
        return (typeof(DynamicSlugcatEarCosmeticSprite), new DynamicSlugcatEarCosmeticSprite.Properties()
        {
            spriteLayerGroups = new SpriteLayerGroup[] { new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Ears, 0) },
            spriteName = "ccgSlugcatEar",
            anglePositions = PlayerGraphicsCCGData.DefaultVanillaRightEarAnglePositions,
            earColor = new Color(1f, 1f, 1f, 1f), // MR7: Default color is white.
            rad = 5f, // MR7: Default radius of ear is 5 pixels.
            scaleX = -1,
            side = 1
        });
    }

    public static (Type, DynamicSlugcatFaceCosmeticSprite.Properties) VanillaSlugcatNoseCosmetic()
    {
        return (typeof(DynamicSlugcatFaceCosmeticSprite), new DynamicSlugcatFaceCosmeticSprite.Properties()
        {
            spriteLayerGroups = new SpriteLayerGroup[] { new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Nose, 0) },
            spriteName = "ccgSlugcatNose",
            anglePositions = PlayerGraphicsCCGData.DefaultVanillaNoseAnglePositions,
            scaleX = 1,
            snapValue = 15, // MR7: Default snap value for nose is 15.
        });
    }

    public static (Type, DynamicSlugcatFaceCosmeticSprite.Properties) VanillaSlugcatLeftEye()
    {
        return (typeof(DynamicSlugcatFaceCosmeticSprite), new DynamicSlugcatFaceCosmeticSprite.Properties()
        {
            spriteLayerGroups = new SpriteLayerGroup[] { new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Eyes, 0) },
            spriteName = "ccgSlugcatEye",
            anglePositions = PlayerGraphicsCCGData.DefaultVanillaLeftEyeAnglePositions,
            scaleX = 1,
            snapValue = 15, // MR7: Default snap value for nose is 15.
        });
    }

    public static (Type, DynamicSlugcatFaceCosmeticSprite.Properties) VanillaSlugcatRightEyeProperties()
    {
        return (typeof(DynamicSlugcatFaceCosmeticSprite), new DynamicSlugcatFaceCosmeticSprite.Properties(new SpriteLayerGroup[]{
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.Eyes, 0)})
        {
            spriteName = "ccgSlugcatNose",
            anglePositions = PlayerGraphicsCCGData.DefaultVanillaNoseAnglePositions,
            scaleX = 1,
            snapValue = 15, // MR7: Default snap value for nose is 15.
        });
    }
    */
}
