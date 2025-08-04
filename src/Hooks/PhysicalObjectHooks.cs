namespace CompartmentalizedCreatureGraphics;

internal static class PhysicalObjectHooks
{
    internal static void PhysicalObject_InitiateGraphicsModule(On.PhysicalObject.orig_InitiateGraphicsModule orig, PhysicalObject self)
    {
        orig(self);

        if (self.graphicsModule != null)
        {
            self.graphicsModule.AddDynamicCreatureCosmeticsToRoom();
        }
    }

    internal static void PhysicalObject_RemoveGraphicsModule(On.PhysicalObject.orig_RemoveGraphicsModule orig, PhysicalObject self)
    {
        orig(self);

        if (self.graphicsModule != null)
        {
            self.graphicsModule.RemoveDynamicCreatureCosmeticsFromRoom();
        }
    }
}
