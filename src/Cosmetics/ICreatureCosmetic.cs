using CompartmentalizedCreatureGraphics.Cosmetics;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

public interface ICreatureCosmetic : IModifySpriteLeaser
{
    public RoomCamera.SpriteLeaser? SLeaser{ get; }

    public SpriteLayerGroup[] SpriteLayerGroups { get; }

    public SpriteEffectGroup[] SpriteEffectGroups { get; }

    public void PostWearerUpdate();

    //-- MS7: TODO: There is probably a miniscule bit of unneccessary overhead that the OnWearer* functions are not in DynamicCosmetics exclusively, but I don't think it's worth it to change it right now.
    // CreatureCosmeticGraphicsReferences will never need the OnWearer* functions for example, yet are still ran in updates.

    public void PostWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk);

    public void PostWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact);
}

public static class ICosmeticExtension
{
    public static bool IsEquipped(this IDynamicCreatureCosmetic cosmetic)
    {
        return cosmetic.Wearer != null;
    }

    //
    // Layer Stuff
    //

    public static int GetLayerGroupIndexForLayer(this ICreatureCosmetic cosmetic, int layer)
    {
        for (int i = 0; i < cosmetic.SpriteLayerGroups.Length; i++)
        {
            if (cosmetic.SpriteLayerGroups[i].Layer == layer)
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
        return creatureCosmetic.SLeaser.sprites[0];
    }

    public static FSprite GetLastSprite(this ICreatureCosmetic creatureCosmetic)
    {
        return creatureCosmetic.SLeaser.sprites[creatureCosmetic.SLeaser.sprites.Length - 1];
    }
}