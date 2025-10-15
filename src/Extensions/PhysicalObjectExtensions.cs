namespace CompartmentalizedCreatureGraphics.Extensions;

public static class PhysicalObjectExtensions
{
    /*
    internal static void AddDynamicCreatureCosmeticsToDrawableObjects(this PhysicalObject physicalObject)
    {
        if (self.owner.room == null)
        {
            return;
        }

        Plugin.LogCCGDebug("Adding cosmetics to drawable objects");

        var wearerCCGData = self.GetGraphicsModuleCCGData();

        for (int i = 0; i < wearerCCGData.cosmetics.Count; i++)
        {
            if (wearerCCGData.cosmetics[i] is IDrawable cosmeticDrawable)
            {
                self.owner.room.drawableObjects.Add(cosmeticDrawable);

                for (int k = 0; k < self.owner.room.game.cameras.Length; k++)
                {
                    if (self.owner.room.game.cameras[k].room == self.owner.room)
                    {
                        // Ms7: This is what actually makes it go "hey initiate the sprites and stuff!"
                        self.owner.room.game.cameras[k].NewObjectInRoom(cosmeticDrawable);
                    }
                }
            }
        }
    }

    internal static void RemoveDynamicCreatureCosmeticsFromDrawableObjects(this PhysicalObject physicalObject)
    {
        if (self.owner.room == null)
            return;

        var wearerCCGData = self.GetGraphicsModuleCCGData();


        for (int i = 0; i < wearerCCGData.cosmetics.Count; i++)
        {
            if (wearerCCGData.cosmetics[i] is IDrawable cosmeticDrawable)
            {
                self.owner.room.drawableObjects.Remove(cosmeticDrawable);
            }
        }
    }
    */
}
