namespace CompartmentalizedCreatureGraphics;

internal static class PhysicalObjectHooks
{
    internal static void ApplyHooks()
    {
        On.PhysicalObject.InitiateGraphicsModule += PhysicalObjectHooks.PhysicalObject_InitiateGraphicsModule;
        On.PhysicalObject.RemoveGraphicsModule += PhysicalObjectHooks.PhysicalObject_RemoveGraphicsModule;
    }

    internal static void RemoveHooks()
    {
        On.PhysicalObject.InitiateGraphicsModule -= PhysicalObjectHooks.PhysicalObject_InitiateGraphicsModule;
        On.PhysicalObject.RemoveGraphicsModule -= PhysicalObjectHooks.PhysicalObject_RemoveGraphicsModule;
    }

    internal static void PhysicalObject_InitiateGraphicsModule(On.PhysicalObject.orig_InitiateGraphicsModule orig, PhysicalObject self)
    {
        orig(self);

        if (self.graphicsModule != null)
        {
            self.graphicsModule.AddDynamicCreatureCosmeticsToDrawableObjects();
        }
    }

    internal static void PhysicalObject_RemoveGraphicsModule(On.PhysicalObject.orig_RemoveGraphicsModule orig, PhysicalObject self)
    {
        orig(self);

        if (self.graphicsModule != null)
        {
            self.graphicsModule.RemoveDynamicCreatureCosmeticsFromDrawableObjects();
        }
    }
}
