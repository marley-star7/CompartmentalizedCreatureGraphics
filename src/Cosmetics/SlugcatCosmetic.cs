namespace CompartmentalizedCreatureGraphics;

public class SlugcatCosmetic : Cosmetic
{
    public Player? player;

    public override void Equip(Creature wearer)
    {
        this.player = (Player)wearer;
        base.Equip(wearer);
    }
}
