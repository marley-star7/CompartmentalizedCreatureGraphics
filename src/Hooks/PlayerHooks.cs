namespace CompartmentalizedCreatureGraphics;

internal static class PlayerHooks
{
    internal static void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {
        orig(player, chunk, direction, speed, firstContact);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetGraphicsModuleCCGData();
        for (int i = 0; i < ccgData.cosmetics.Count; i++)
        {
            ccgData.cosmetics[i].OnWearerTerrainImpact(player, chunk, direction, speed, firstContact);
        }
    }

    internal static void Player_Collide(On.Player.orig_Collide orig, Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {
        orig(player, otherObject, myChunk, otherChunk);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetGraphicsModuleCCGData();
        for (int i = 0; i < ccgData.cosmetics.Count; i++)
        {
            ccgData.cosmetics[i].OnWearerCollide(player, otherObject, myChunk, otherChunk);
        }
    }
}

