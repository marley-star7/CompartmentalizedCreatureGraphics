namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmetic : DynamicCreatureCosmetic
{
    public Player Player => (Player)Wearer;

    public DynamicSlugcatCosmetic(Player wearer, DynamicCreatureCosmetic.Properties properties) : base(wearer, properties)
    {
    }
}
