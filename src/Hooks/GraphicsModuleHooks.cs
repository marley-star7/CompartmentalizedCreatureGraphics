using CompartmentalizedCreatureGraphics.Extensions;

internal static class GraphicsModuleHooks
{
    internal static void ApplyHooks()
    {
        On.GraphicsModule.Update += GraphicsModule_Update;

        On.GraphicsModule.InitiateSprites += GraphicsModuleHooks.GraphicsModule_InitiateSprites;
        On.GraphicsModule.DrawSprites += GraphicsModuleHooks.GraphicsModule_DrawSprites;
        On.GraphicsModule.ApplyPalette += GraphicsModuleHooks.GraphicsModule_ApplyPalette;
        On.GraphicsModule.AddToContainer += GraphicsModuleHooks.GraphicsModule_AddToContainer;
    }

    internal static void RemoveHooks()
    {
        On.GraphicsModule.Update -= GraphicsModule_Update;

        On.GraphicsModule.InitiateSprites -= GraphicsModuleHooks.GraphicsModule_InitiateSprites;
        On.GraphicsModule.DrawSprites -= GraphicsModuleHooks.GraphicsModule_DrawSprites;
        On.GraphicsModule.ApplyPalette -= GraphicsModuleHooks.GraphicsModule_ApplyPalette;
        On.GraphicsModule.AddToContainer -= GraphicsModuleHooks.GraphicsModule_AddToContainer;
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

    internal static void GraphicsModule_InitiateSprites(On.GraphicsModule.orig_InitiateSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser SLeaser, RoomCamera rCam)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = SLeaser;

        orig(self, SLeaser, rCam);
        self.ReorderDynamicCosmetics();

        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            cosmetics[i].PostWearerInitiateSprites(SLeaser, rCam);
        }
    }

    internal static void GraphicsModule_DrawSprites(On.GraphicsModule.orig_DrawSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser SLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = SLeaser;

        orig(self, SLeaser, rCam, timeStacker, camPos);

        if (SLeaser.deleteMeNextFrame)
        {
            CleanUpDynamicCosmetics(data);
        }
        else
        {
            DrawDynamicCosmetics(data, SLeaser, rCam, timeStacker, camPos);
        }
    }

    private static void CleanUpDynamicCosmetics(GraphicsModuleCCGData data)
    {
        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            if (cosmetics[i] is DynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic.SLeaser != null)
            {
                dynamicCosmetic.SLeaser.CleanSpritesAndRemove();
            }
        }
    }

    private static void DrawDynamicCosmetics(GraphicsModuleCCGData data, RoomCamera.SpriteLeaser SLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            cosmetics[i].PostWearerDrawSprites(SLeaser, rCam, timeStacker, camPos);
        }
    }

    internal static void GraphicsModule_ApplyPalette(On.GraphicsModule.orig_ApplyPalette orig, GraphicsModule self, RoomCamera.SpriteLeaser SLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = SLeaser;

        orig(self, SLeaser, rCam, palette);
        ApplyPaletteToCosmetics(data, SLeaser, rCam, palette);
    }

    private static void ApplyPaletteToCosmetics(GraphicsModuleCCGData data, RoomCamera.SpriteLeaser SLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            cosmetics[i].PostWearerApplyPalette(SLeaser, rCam, in palette);
        }
    }

    internal static void GraphicsModule_AddToContainer(On.GraphicsModule.orig_AddToContainer orig, GraphicsModule self, RoomCamera.SpriteLeaser SLeaser, RoomCamera rCam, FContainer newContainer)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = SLeaser;

        orig(self, SLeaser, rCam, newContainer);
        self.AddDynamicCosmeticsToContainer(SLeaser, rCam, newContainer);
    }
}