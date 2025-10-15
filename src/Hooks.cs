namespace CompartmentalizedCreatureGraphics;

public static class Hooks
{
    internal static void ApplyHooks()
    {
        PhysicalObjectHooks.ApplyHooks();
        GraphicsModuleHooks.ApplyHooks();

        PlayerHooks.ApplyHooks();
        ApplyPlayerGraphicsHooks();
    }

    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        PhysicalObjectHooks.RemoveHooks();
        GraphicsModuleHooks.RemoveHooks();

        PlayerHooks.RemoveHooks();
        RemovePlayerGraphicsHooks();
    }

    private static void ApplyPlayerGraphicsHooks()
    {
        On.PlayerGraphics.ctor += PlayerGraphicsHooks.PlayerGraphics_ctor;
        On.PlayerGraphics.Update += PlayerGraphicsHooks.PlayerGraphics_Update;

        On.PlayerGraphics.DrawSprites += PlayerGraphicsHooks.PlayerGraphics_DrawSprites;
    }

    private static void RemovePlayerGraphicsHooks()
    {
        On.PlayerGraphics.ctor -= PlayerGraphicsHooks.PlayerGraphics_ctor;
        On.PlayerGraphics.Update -= PlayerGraphicsHooks.PlayerGraphics_Update;

        On.PlayerGraphics.DrawSprites -= PlayerGraphicsHooks.PlayerGraphics_DrawSprites;
    }
}
