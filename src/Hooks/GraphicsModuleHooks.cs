using CompartmentalizedCreatureGraphics.Extensions;

internal static class GraphicsModuleHooks
{
    internal static void GraphicsModule_InitiateSprites(On.GraphicsModule.orig_InitiateSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = sLeaser;

        orig(self, sLeaser, rCam);
        self.ReorderDynamicCosmetics();
    }

    internal static void GraphicsModule_DrawSprites(On.GraphicsModule.orig_DrawSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, timeStacker, camPos);

        if (sLeaser.deleteMeNextFrame)
        {
            CleanUpDynamicCosmetics(data);
        }
        else
        {
            DrawDynamicCosmetics(data, sLeaser, rCam, timeStacker, camPos);
        }
    }

    private static void CleanUpDynamicCosmetics(GraphicsModuleCCGData data)
    {
        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            if (cosmetics[i] is DynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic.sLeaser != null)
            {
                dynamicCosmetic.sLeaser.CleanSpritesAndRemove();
            }
        }
    }

    private static void DrawDynamicCosmetics(GraphicsModuleCCGData data, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            cosmetics[i].OnWearerDrawSprites(sLeaser, rCam, timeStacker, camPos);
        }
    }

    internal static void GraphicsModule_ApplyPalette(On.GraphicsModule.orig_ApplyPalette orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, palette);
        ApplyPaletteToCosmetics(data, sLeaser, rCam, palette);
    }

    private static void ApplyPaletteToCosmetics(GraphicsModuleCCGData data, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var cosmetics = data.cosmetics;
        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            cosmetics[i].OnWearerApplyPalette(sLeaser, rCam, in palette);
        }
    }

    internal static void GraphicsModule_AddToContainer(On.GraphicsModule.orig_AddToContainer orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        var data = self.GetGraphicsModuleCCGData();
        data.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, newContainer);
        self.AddDynamicCosmeticsToContainer(sLeaser, rCam, newContainer);
    }
}