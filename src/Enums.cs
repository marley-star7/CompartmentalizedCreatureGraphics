namespace CompartmentalizedCreatureGraphics;

public static class CCGEnums
{
    // The Golden Sheet of Sprite Bull-Sheet.
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
        Eyes,
        Nose,
        FaceMask,
        // Should be infront of almost all.
        BaseLeftTerrainHand,
        BaseRightTerrainHand,
    }
}
