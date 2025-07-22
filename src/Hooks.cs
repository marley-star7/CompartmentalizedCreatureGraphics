namespace CompartmentalizedCreatureGraphics;

public static class Hooks
{
    internal static void ApplyHooks()
    {
        ApplyPlayerHooks();
        ApplyPlayerGraphicsHooks();
        ApplyLizardGraphicsHooks();
    }

    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        RemovePlayerHooks();
        RemovePlayerGraphicsHooks();
        RemoveLizardGraphicsHooks();
    }

    private static void ApplyPlayerHooks()
    {
        On.Player.Collide += PlayerHooks.Player_Collide;
        On.Player.TerrainImpact += PlayerHooks.Player_TerrainImpact;
    }

    private static void RemovePlayerHooks()
    {
        On.Player.Collide += PlayerHooks.Player_Collide;
        On.Player.TerrainImpact += PlayerHooks.Player_TerrainImpact;
    }

    private static void ApplyPlayerGraphicsHooks()
    {
        On.PlayerGraphics.ctor += PlayerGraphicsHooks.PlayerGraphics_ctor;
        On.PlayerGraphics.Update += PlayerGraphicsHooks.PlayerGraphics_Update;

        On.PlayerGraphics.InitiateSprites += PlayerGraphicsHooks.PlayerGraphics_InitiateSprites;
        On.PlayerGraphics.DrawSprites += PlayerGraphicsHooks.PlayerGraphics_DrawSprites;
        On.PlayerGraphics.ApplyPalette += PlayerGraphicsHooks.PlayerGraphics_ApplyPalette;
        On.PlayerGraphics.AddToContainer += PlayerGraphicsHooks.PlayerGraphics_AddToContainer;
    }

    private static void RemovePlayerGraphicsHooks()
    {
        On.PlayerGraphics.ctor -= PlayerGraphicsHooks.PlayerGraphics_ctor;
        On.PlayerGraphics.Update -= PlayerGraphicsHooks.PlayerGraphics_Update;

        On.PlayerGraphics.InitiateSprites -= PlayerGraphicsHooks.PlayerGraphics_InitiateSprites;
        On.PlayerGraphics.DrawSprites -= PlayerGraphicsHooks.PlayerGraphics_DrawSprites;
        On.PlayerGraphics.ApplyPalette -= PlayerGraphicsHooks.PlayerGraphics_ApplyPalette;
        On.PlayerGraphics.AddToContainer -= PlayerGraphicsHooks.PlayerGraphics_AddToContainer;
    }

    private static void ApplyLizardGraphicsHooks()
    {
        On.LizardGraphics.InitiateSprites += LizardGraphicsHooks.LizardGraphics_InitiateSprites;
    }

    private static void RemoveLizardGraphicsHooks()
    {
        On.LizardGraphics.InitiateSprites -= LizardGraphicsHooks.LizardGraphics_InitiateSprites;
    }
}
