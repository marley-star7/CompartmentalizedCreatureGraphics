
namespace CompartmentalizedCreatureGraphics;

public class SpriteLeaserHooks
{
    // We hook straight into the spriteLeaser to bypass all the other junk, ensuring our the cosmetic updates always occur.
    // By running our updates also RIGHT after the spriteLeasers, we ensure it's at least as somewhat nearby in cache as was recently used.

    internal static void ApplyHooks()
    {
        On.RoomCamera.SpriteLeaser.ctor += SpriteLeaser_ctor;
        On.RoomCamera.SpriteLeaser.Update += SpriteLeaser_Update;
        On.RoomCamera.SpriteLeaser.UpdatePalette += SpriteLeaser_UpdatePalette;
        On.RoomCamera.SpriteLeaser.AddSpritesToContainer += SpriteLeaser_AddSpritesToContainer;
        On.RoomCamera.SpriteLeaser.RemoveAllSpritesFromContainer += SpriteLeaser_RemoveAllSpritesFromContainer;
        On.RoomCamera.SpriteLeaser.CleanSpritesAndRemove += SpriteLeaser_CleanSpritesAndRemove;
    }

    internal static void RemoveHooks()
    {
        On.RoomCamera.SpriteLeaser.ctor -= SpriteLeaser_ctor;
        On.RoomCamera.SpriteLeaser.Update -= SpriteLeaser_Update;
        On.RoomCamera.SpriteLeaser.UpdatePalette -= SpriteLeaser_UpdatePalette;
        On.RoomCamera.SpriteLeaser.AddSpritesToContainer -= SpriteLeaser_AddSpritesToContainer;
        On.RoomCamera.SpriteLeaser.RemoveAllSpritesFromContainer -= SpriteLeaser_RemoveAllSpritesFromContainer;
        On.RoomCamera.SpriteLeaser.CleanSpritesAndRemove -= SpriteLeaser_CleanSpritesAndRemove;
    }

    private static void SpriteLeaser_ctor(On.RoomCamera.SpriteLeaser.orig_ctor orig, RoomCamera.SpriteLeaser self, IDrawable obj, RoomCamera rCam)
    {
        orig(self, obj, rCam);

        if (obj is GraphicsModule graphicsModule)
        {
            var graphicsModuleData = graphicsModule.GetGraphicsModuleCCGData();
            graphicsModuleData.sLeaser = self;

            // Initiate our cosmetics sprites.
            var cosmetics = graphicsModuleData.cosmetics;
            for (int i = 0, count = cosmetics.Count; i < count; i++)
            {
                cosmetics[i].PostWearerInitiateSprites(self, rCam);
            }

            // Palette is also applied on spriteLeaser creation, so have to run it here too.
            ApplyCosmeticsPalettes(ref graphicsModuleData, ref self, ref rCam);

            // And then reorder duh stuff.
            graphicsModule.ReorderDynamicCosmetics();
        }
    }

    private static void SpriteLeaser_Update(On.RoomCamera.SpriteLeaser.orig_Update orig, RoomCamera.SpriteLeaser self, float timeStacker, RoomCamera rCam, Vector2 camPos)
    {
        orig(self, timeStacker, rCam, camPos);

        if (self.drawableObject is GraphicsModule graphicsModule)
        {
            var graphicsModuleData = graphicsModule.GetGraphicsModuleCCGData();
            graphicsModuleData.sLeaser = self;

            var cosmetics = graphicsModuleData.cosmetics;
            for (int i = 0, count = cosmetics.Count; i < count; i++)
            {
                cosmetics[i].PostWearerDrawSprites(self, rCam, timeStacker, camPos);
            }
        }
    }

    private static void SpriteLeaser_UpdatePalette(On.RoomCamera.SpriteLeaser.orig_UpdatePalette orig, RoomCamera.SpriteLeaser self, RoomCamera rCam, RoomPalette palette)
    {
        orig(self, rCam, palette);

        if (self.drawableObject is GraphicsModule graphicsModule)
        {
            var graphicsModuleData = graphicsModule.GetGraphicsModuleCCGData();
            graphicsModuleData.sLeaser = self;

            ApplyCosmeticsPalettes(ref graphicsModuleData, ref self, ref rCam);
        }
    }

    private static void SpriteLeaser_AddSpritesToContainer(On.RoomCamera.SpriteLeaser.orig_AddSpritesToContainer orig, RoomCamera.SpriteLeaser self, FContainer newContainer, RoomCamera rCam)
    {
        orig(self, newContainer, rCam);

        if (self.drawableObject is GraphicsModule graphicsModule)
        {
            var graphicsModuleData = graphicsModule.GetGraphicsModuleCCGData();
            graphicsModuleData.sLeaser = self;

            Plugin.LogDebug($"Adding {graphicsModuleData.cosmetics.Count} dynamic cosmetics to container.");

            graphicsModule.ReorderDynamicCosmetics();
        }
    }

    private static void SpriteLeaser_RemoveAllSpritesFromContainer(On.RoomCamera.SpriteLeaser.orig_RemoveAllSpritesFromContainer orig, RoomCamera.SpriteLeaser self)
    {
        if (self.drawableObject is GraphicsModule graphicsModule)
        {
            var graphicsModuleData = graphicsModule.GetGraphicsModuleCCGData();
            graphicsModuleData.sLeaser = self;

            var cosmetics = graphicsModuleData.cosmetics;
            for (int i = 0, count = cosmetics.Count; i < count; i++)
            {
                RemoveCosmeticSpritesFromContainer(cosmetics[i]);
            }
        }

        orig(self);
    }

    private static void RemoveCosmeticSpritesFromContainer(ICreatureCosmetic dynamicCreatureCosmetic)
    {
        var sLeaser = dynamicCreatureCosmetic.SLeaser;
        if (sLeaser == null)
        {
            return;
        }

        if (sLeaser.sprites != null)
        {
            for (int i = 0; i < sLeaser.sprites.Length; i++)
            {
                sLeaser.sprites[i].RemoveFromContainer();
            }
        }
        if (sLeaser.containers != null)
        {
            for (int j = 0; j < sLeaser.containers.Length; j++)
            {
                sLeaser.containers[j].RemoveFromContainer();
            }
        }
    }

    private static void SpriteLeaser_CleanSpritesAndRemove(On.RoomCamera.SpriteLeaser.orig_CleanSpritesAndRemove orig, RoomCamera.SpriteLeaser self)
    {
        if (self.drawableObject is GraphicsModule graphicsModule)
        {
            var graphicsModuleData = graphicsModule.GetGraphicsModuleCCGData();
            graphicsModuleData.sLeaser = self;

            var cosmetics = graphicsModuleData.cosmetics;
            for (int i = 0, count = cosmetics.Count; i < count; i++)
            {
                if (cosmetics[i] is DynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic.SLeaser != null)
                {
                    dynamicCosmetic.SLeaser.deleteMeNextFrame = true;
                    // Don't run CleanSpritesAndRemove because it would then run RemoveAllSprites twice.
                }
            }
        }

        orig(self);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ApplyCosmeticsPalettes(ref GraphicsModuleCCGData graphicsModuleData, ref RoomCamera.SpriteLeaser spriteLeaser, ref RoomCamera roomCamera)
    {
        var cosmetics = graphicsModuleData.cosmetics;

        for (int i = 0, count = cosmetics.Count; i < count; i++)
        {
            if (cosmetics[i] is DynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic.SLeaser != null)
            {
                dynamicCosmetic.PostWearerApplyPalette(spriteLeaser, roomCamera, roomCamera.currentPalette);
            }
        }
    }
}
