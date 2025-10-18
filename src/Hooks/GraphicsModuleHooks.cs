using CompartmentalizedCreatureGraphics.Extensions;

internal static class GraphicsModuleHooks
{
    internal static void ApplyHooks()
    {
        On.GraphicsModule.Update += GraphicsModule_Update;
    }

    internal static void RemoveHooks()
    {
        On.GraphicsModule.Update -= GraphicsModule_Update;
    }

    private static void GraphicsModule_Update(On.GraphicsModule.orig_Update orig, GraphicsModule self)
    {
        orig(self);

        var data = self.GetGraphicsModuleCCGData();

        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            cosmetics[i].PostWearerUpdate();
        }
    }
}