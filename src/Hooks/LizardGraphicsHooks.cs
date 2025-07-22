using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics;

internal static class LizardGraphicsHooks
{
    internal static void LizardGraphics_InitiateSprites(On.LizardGraphics.orig_InitiateSprites orig, LizardGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        orig(self, sLeaser, rCam);
        // Save on iniate of sprites the sprite leaser for the lizard.
        self.GetGraphicsModuleCCGData().sLeaser = sLeaser;
    }
}
