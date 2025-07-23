namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmetic : DynamicCosmetic
{
    public Player? player;

    public DynamicSlugcatCosmetic(SpriteLayerGroup[] spriteLayerGroups) : base(spriteLayerGroups)
    {
    }

    public override void Equip(Creature wearer)
    {
        player = (Player)wearer;
        base.Equip(wearer);
    }
}
