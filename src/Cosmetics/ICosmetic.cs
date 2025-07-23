using CompartmentalizedCreatureGraphics.Cosmetics;

namespace CompartmentalizedCreatureGraphics;

public interface ICosmetic
{
    public RoomCamera.SpriteLeaser SpriteLeaser{ get; }

    public SpriteLayerGroup[] SpriteLayerGroups { get; set; }

    public void Equip(Creature wearer);

    public void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos);

    //-- MR7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    public  void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette);

    public void OnWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk);

    public void OnWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact);
}

public static class ICosmeticExtension
{
    public static int GetLayerGroupIndexForLayer(this ICosmetic cosmetic, int layer)
    {
        for (int i = 0; i < cosmetic.SpriteLayerGroups.Length; i++)
        {
            if (cosmetic.SpriteLayerGroups[i].layer == layer)
            {
                return i;
            }
        }
        return -1; // Not Found
    }

    public static void SetLayerGroupNeedsReorder(this ICosmetic cosmetic, int layer, bool value)
    {
        cosmetic.SpriteLayerGroups[cosmetic.GetLayerGroupIndexForLayer(layer)].needsReorder = value;
    }

    public static void SetLayerGroupsNeedsReorder(this ICosmetic cosmetic, bool value)
    {
        for (int i = 0; i < cosmetic.SpriteLayerGroups.Length; i++)
        {
            cosmetic.SpriteLayerGroups[i].needsReorder = value;
        }
    }
}