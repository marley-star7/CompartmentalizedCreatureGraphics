namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmetic : DynamicCreatureCosmetic
{
    public Player player => (Player)wearer;

    public DynamicSlugcatCosmetic(PlayerGraphics playerGraphics, DynamicCreatureCosmetic.Properties properties) : base(playerGraphics, properties)
    {
    }
}
