namespace CompartmentalizedCreatureGraphics;

public static class Enums
{
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
        BaseTail,
        BaseLegs,
        BaseBody,
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
