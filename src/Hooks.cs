namespace CompartmentalizedCreatureGraphics;

public static class Hooks
{
    internal static void ApplyHooks()
    {
        ApplyPhysicalObjectHooks();
        ApplyGraphicsModuleHooks();

        ApplyPlayerHooks();
        ApplyPlayerGraphicsHooks();

        ApplyLizardGraphicsHooks();
    }

    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        RemovePhysicalObjectHooks();
        RemoveGraphicsModuleHooks();

        RemovePlayerHooks();
        RemovePlayerGraphicsHooks();

        RemoveLizardGraphicsHooks();
    }

    private static void ApplyPhysicalObjectHooks()
    {
        On.PhysicalObject.InitiateGraphicsModule += PhysicalObjectHooks.PhysicalObject_InitiateGraphicsModule;
        On.PhysicalObject.RemoveGraphicsModule += PhysicalObjectHooks.PhysicalObject_RemoveGraphicsModule;
    }

    private static void RemovePhysicalObjectHooks()
    {
        On.PhysicalObject.InitiateGraphicsModule -= PhysicalObjectHooks.PhysicalObject_InitiateGraphicsModule;
        On.PhysicalObject.RemoveGraphicsModule -= PhysicalObjectHooks.PhysicalObject_RemoveGraphicsModule;
    }

    private static void ApplyGraphicsModuleHooks()
    {
        On.GraphicsModule.InitiateSprites += GraphicsModuleHooks.GraphicsModule_InitiateSprites;
        On.GraphicsModule.DrawSprites += GraphicsModuleHooks.GraphicsModule_DrawSprites;
        On.GraphicsModule.ApplyPalette += GraphicsModuleHooks.GraphicsModule_ApplyPalette;
        On.GraphicsModule.AddToContainer += GraphicsModuleHooks.GraphicsModule_AddToContainer;
    }

    private static void RemoveGraphicsModuleHooks()
    {
        On.GraphicsModule.InitiateSprites -= GraphicsModuleHooks.GraphicsModule_InitiateSprites;
        On.GraphicsModule.DrawSprites -= GraphicsModuleHooks.GraphicsModule_DrawSprites;
        On.GraphicsModule.ApplyPalette -= GraphicsModuleHooks.GraphicsModule_ApplyPalette;
        On.GraphicsModule.AddToContainer -= GraphicsModuleHooks.GraphicsModule_AddToContainer;
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
