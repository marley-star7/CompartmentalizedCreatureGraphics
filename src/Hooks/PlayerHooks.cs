namespace CompartmentalizedCreatureGraphics;

internal static class PlayerHooks
{
    internal static void ApplyHooks()
    {
        On.Player.InitiateGraphicsModule += Player_InitiateGraphicsModule;

        On.Player.Collide += PlayerHooks.Player_Collide;
        On.Player.TerrainImpact += PlayerHooks.Player_TerrainImpact;
    }

    // Have to do it here because of how hooking woorks YAYYY (find a workaround fuck fuck fucck fuc kfuckcuamoic opiasn fioguSD ojhgkpaiondfg ,nioasdebfouhgaobnuiyderf,)
    internal static void Player_InitiateGraphicsModule(On.Player.orig_InitiateGraphicsModule orig, Player self)
    {
        orig(self);

        if (self.graphicsModule != null)
        {
            self.graphicsModule.AddDynamicCreatureCosmeticsToDrawableObjects();
        }
    }

    internal static void RemoveHooks()
    {
        On.Player.Collide += PlayerHooks.Player_Collide;
        On.Player.TerrainImpact += PlayerHooks.Player_TerrainImpact;
    }

    internal static void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {
        orig(player, chunk, direction, speed, firstContact);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetGraphicsModuleCCGData();
        for (int i = 0; i < ccgData.cosmetics.Count; i++)
        {
            ccgData.cosmetics[i].PostWearerTerrainImpact(player, chunk, direction, speed, firstContact);
        }
    }

    internal static void Player_Collide(On.Player.orig_Collide orig, Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {
        orig(player, otherObject, myChunk, otherChunk);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetGraphicsModuleCCGData();
        for (int i = 0; i < ccgData.cosmetics.Count; i++)
        {
            ccgData.cosmetics[i].PostWearerCollide(player, otherObject, myChunk, otherChunk);
        }
    }
}

