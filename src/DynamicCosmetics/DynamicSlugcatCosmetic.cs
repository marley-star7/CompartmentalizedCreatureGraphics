namespace CompartmentalizedCreatureGraphics.SlugcatCosmetics;

public class DynamicSlugcatCosmetic : DynamicCosmetic
{
    public Player? player;

    public override void Equip(Creature wearer)
    {
        this.player = (Player)wearer;
        base.Equip(wearer);
    }
}
