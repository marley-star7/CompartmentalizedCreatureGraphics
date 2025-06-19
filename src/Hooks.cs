using UnityEngine;

namespace CompartmentalizedCreatureGraphics;

public static partial class Hooks
{
    internal static void ApplyHooks()
    {
        ApplyPlayerHooks();
        ApplyPlayerGraphicsHooks();
    }

    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        RemovePlayerHooks();
        RemovePlayerGraphicsHooks();
    }
}