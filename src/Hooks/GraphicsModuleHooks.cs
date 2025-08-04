
using CompartmentalizedCreatureGraphics.Extensions;

internal static class GraphicsModuleHooks
{
    internal static void GraphicsModule_InitiateSprites(On.GraphicsModule.orig_InitiateSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        var selfCCGData = self.GetGraphicsModuleCCGData();
        selfCCGData.sLeaser = sLeaser;

        for (int i = 0; i < selfCCGData.cosmetics.Count; i++)
        {
            self.AddDynamicCreatureCosmeticsToRoom();
        }

        orig(self, sLeaser, rCam);
    }

    internal static void GraphicsModule_DrawSprites(On.GraphicsModule.orig_DrawSprites orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var selfCCGData = self.GetGraphicsModuleCCGData();
        selfCCGData.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, timeStacker, camPos);

        for (int i = 0; i < selfCCGData.cosmetics.Count; i++)
        {
            selfCCGData.cosmetics[i].OnWearerDrawSprites(sLeaser, rCam, timeStacker, camPos);
        }
    }

    internal static void GraphicsModule_ApplyPalette(On.GraphicsModule.orig_ApplyPalette orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var selfCCGData = self.GetGraphicsModuleCCGData();
        selfCCGData.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, palette);

        for (int i = 0; i < selfCCGData.cosmetics.Count; i++)
        {
            selfCCGData.cosmetics[i].OnWearerApplyPalette(sLeaser, rCam, in palette);
        }
    }

    internal static void GraphicsModule_AddToContainer(On.GraphicsModule.orig_AddToContainer orig, GraphicsModule self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        var selfCCGData = self.GetGraphicsModuleCCGData();
        selfCCGData.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, newContainer);

        self.AddDynamicCosmeticsToContainer(sLeaser, rCam, newContainer);
    }
}