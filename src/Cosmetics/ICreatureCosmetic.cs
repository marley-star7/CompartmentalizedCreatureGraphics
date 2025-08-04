using CompartmentalizedCreatureGraphics.Cosmetics;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

public interface ICreatureCosmetic
{
    public RoomCamera.SpriteLeaser? sLeaser{ get; }

    public SpriteLayerGroup[] spriteLayerGroups { get; }

    //-- MR7: TODO: There is probably a miniscule bit of unneccessary overhead that the OnWearer* functions are not in DynamicCosmetics exclusively, but I don't think it's worth it to change it right now.
    // CreatureCosmeticGraphicsReferences will never need the OnWearer* functions for example, yet are still ran in updates.

    public void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos);

    /// <summary>
    /// -- MR7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    /// </summary>
    /// <param name="wearerSLeaser"></param>
    /// <param name="rCam"></param>
    /// <param name="palette"></param>
    public void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette);

    public void OnWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk);

    public void OnWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact);
}

public static class ICosmeticExtension
{
    public static bool IsEquipped(this IDynamicCreatureCosmetic cosmetic)
    {
        return cosmetic.wearer != null;
    }

    //
    // Layer Stuff
    //

    public static int GetLayerGroupIndexForLayer(this ICreatureCosmetic cosmetic, int layer)
    {
        for (int i = 0; i < cosmetic.spriteLayerGroups.Length; i++)
        {
            if (cosmetic.spriteLayerGroups[i].layer == layer)
            {
                return i;
            }
        }
        return -1; // Not Found
    }
    
    //
    // Utility Functions
    //

    public static FSprite GetFirstSprite(this ICreatureCosmetic creatureCosmetic)
    {
        return creatureCosmetic.sLeaser.sprites[0];
    }

    public static FSprite GetLastSprite(this ICreatureCosmetic creatureCosmetic)
    {
        return creatureCosmetic.sLeaser.sprites[creatureCosmetic.sLeaser.sprites.Length - 1];
    }
}