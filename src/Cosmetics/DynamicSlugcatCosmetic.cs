
namespace CompartmentalizedCreatureGraphics.SlugcatCosmetics;

public class DynamicSlugcatCosmetic : DynamicCosmetic
{
    public Player? player;

    public DynamicSlugcatCosmetic(Dictionary<int, SpriteLayer> spritesLayers) : base(spritesLayers)
    {
    }

    public override void Equip(Creature wearer)
    {
        player = (Player)wearer;
        base.Equip(wearer);
    }
}
