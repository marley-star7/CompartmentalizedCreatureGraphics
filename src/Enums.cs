namespace CompartmentalizedCreatureGraphics;

public static class Enums
{
    public static void Init()
    {

    }

    public class DynamicCosmeticID : ExtEnum<DynamicCosmeticID>
    {
        public DynamicCosmeticID(string value, bool register = false) : base(value, register) { }
    }

    public class DynamicCosmeticPropertiesID : ExtEnum<DynamicCosmeticPropertiesID>
    {
        public DynamicCosmeticPropertiesID(string value, bool register = false) : base(value, register) { }
    }

    public enum SlugcatCosmeticLayer
    {
        None,

        // Should be behind almost all.
        Back,
        BaseTail,

        BaseLegs,
        BaseBody,
        BodyCover,

        BaseLeftArm,
        BaseRightArm,
        BaseHips,

        BaseHead,
        Ear,
        EarCover,

        BaseFace,
        Eye,
        EyeCover,
        Nose,
        FaceMask,

        // Should be infront of almost all.
        BaseLeftTerrainHand,
        BaseRightTerrainHand,
    }
}
