namespace CompartmentalizedCreatureGraphics.Hooks;

public static class PlayerHooks
{
    internal static void ApplyHooks()
    {
        On.Player.Collide += Player_Collide;
        On.Player.TerrainImpact += Player_TerrainImpact;
    }

    internal static void RemoveHooks()
    {
        On.Player.Collide -= Player_Collide;
        On.Player.TerrainImpact -= Player_TerrainImpact;
    }

    private static void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {
        orig(player, chunk, direction, speed, firstContact);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetGraphicsModuleCCGData();
        for (int i = 0; i < ccgData.dynamicCosmetics.Count; i++)
        {
            ccgData.dynamicCosmetics[i].OnWearerTerrainImpact(player, chunk, direction, speed, firstContact);
        }
    }

    private static void Player_Collide(On.Player.orig_Collide orig, Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {
        orig(player, otherObject, myChunk, otherChunk);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetGraphicsModuleCCGData();
        for (int i = 0; i < ccgData.dynamicCosmetics.Count; i++)
        {
            ccgData.dynamicCosmetics[i].OnWearerCollide(player, otherObject, myChunk, otherChunk);
        }
    }
}

