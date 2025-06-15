/*
using UnityEngine;
using RWCustom;

namespace SlugCrafting;

public static partial class Hooks
{
    //-- Add hooks
    internal static void ApplyGraphicsModuleHooks()
    {
        On.PlayerGraphics.InitiateSprites += GraphicsModule_InitiateSprites;
        On.PlayerGraphics.DrawSprites += GraphicsModule_DrawSprites;
    }

    //-- Remove hooks
    internal static void RemoveGraphicsModuleHooks()
    {
        On.PlayerGraphics.InitiateSprites -= GraphicsModule_InitiateSprites;
        On.PlayerGraphics.DrawSprites += GraphicsModule_DrawSprites;
    }

    private static void GraphicsModule_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        self.GetGraphicsModuleCraftingData().sLeaser = sLeaser;
        orig(self, sLeaser, rCam);
    }

    private static void GraphicsModule_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        self.GetGraphicsModuleCraftingData().sLeaser = sLeaser;
        orig(self, sLeaser, rCam, timeStacker, camPos);
    }
}
*/