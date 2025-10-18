namespace CompartmentalizedCreatureGraphics;

public static class Hooks
{
    internal static void ApplyHooks()
    {
        SpriteLeaserHooks.ApplyHooks();

        PhysicalObjectHooks.ApplyHooks();

        PlayerGraphicsHooks.ApplyHooks();
        GraphicsModuleHooks.ApplyHooks();

        PlayerHooks.ApplyHooks();
    }

    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        SpriteLeaserHooks.RemoveHooks();

        PhysicalObjectHooks.RemoveHooks();

        PlayerGraphicsHooks.ApplyHooks();
        GraphicsModuleHooks.RemoveHooks();

        PlayerHooks.RemoveHooks();
    }
}
