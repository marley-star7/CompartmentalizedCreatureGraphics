namespace CompartmentalizedCreatureGraphics;

public static class CCGEnums
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
        BaseFace,
        Ear,
        EarCover,
        Eye,
        EyeCover,
        Nose,
        FaceMask,
        // Should be infront of almost all.
        BaseLeftTerrainHand,
        BaseRightTerrainHand,
    }
}
