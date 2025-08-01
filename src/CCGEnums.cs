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

    // TODO: make extEnum for storing cosmetic preset data rather than using strings in dictionary.
    // TODO: convert this to a list of strings or something, and then add system to allow adding between some.
    // TODO: use a json converter for that, ask deepseek for one.

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
        Ears,
        EarCover,
        Eyes,
        EyeCover,
        Nose,
        FaceMask,
        // Should be infront of almost all.
        BaseLeftTerrainHand,
        BaseRightTerrainHand,
    }
}
