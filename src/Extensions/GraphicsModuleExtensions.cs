using CompartmentalizedCreatureGraphics.Cosmetics;
using IL;
using UnityEngine.PlayerLoop;

namespace CompartmentalizedCreatureGraphics.Extensions;

public class GraphicsModuleCCGData
{
    public RoomCamera.SpriteLeaser? sLeaser;

    /// <summary>
    /// Enable this to use the updated graphics.
    /// </summary>
    public bool compartmentalizedGraphicsEnabled = false;

    //-- MS7: TODO: could swap these lists with swapback arrays for better performance if necessary down the line.

    public List<ICreatureCosmetic> cosmetics = new();
    /// <summary>
    /// Dictionary containing refrences to dynamic cosmetics rendering in a certain layer.
    /// The int key should be the cosmeticLayer Enum of which cosmetic layer we are on.
    /// The value inside refrences a dynamic cosmetic which is using this layer.
    /// </summary>
    public Dictionary<int, List<ICreatureCosmetic>> layersCosmetics = new();

    public WeakReference<GraphicsModule> graphicsModuleRef;

    public GraphicsModuleCCGData(GraphicsModule graphicsModuleRef)
    {
        this.graphicsModuleRef = new WeakReference<GraphicsModule>(graphicsModuleRef);
    }
}

public static class GraphicsModuleCCGExtensions
{
    internal static readonly ConditionalWeakTable<GraphicsModule, GraphicsModuleCCGData> ccgDataConditionalWeakTable = new();

    public static GraphicsModuleCCGData GetGraphicsModuleCCGData(this GraphicsModule graphicsModule)
    {
        return ccgDataConditionalWeakTable.GetValue(graphicsModule, gm =>
        {
            // Check type at creation time
            if (graphicsModule is PlayerGraphics playerGraphics)
                return new PlayerGraphicsCCGData(playerGraphics);
            else
                return new GraphicsModuleCCGData(graphicsModule); // Default case
        });
    }

    public static void AddCreatureCosmetic(this GraphicsModule self, ICreatureCosmetic cosmetic)
    {
        var selfCCGData = self.GetGraphicsModuleCCGData();

        selfCCGData.cosmetics.Add(cosmetic);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < cosmetic.SpriteLayerGroups.Length; i++)
            selfCCGData.layersCosmetics[cosmetic.SpriteLayerGroups[i].Layer].Add(cosmetic);

        if (cosmetic is IDynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic is UpdatableAndDeletable dynamicCosmeticUpdatable)
        {
            self.owner.room.AddObject(dynamicCosmeticUpdatable);
        }
    }

    public static void RemoveCreatureCosmetic(this GraphicsModule self, ICreatureCosmetic cosmetic)
    {
        var selfCCGData = self.GetGraphicsModuleCCGData();

        selfCCGData.cosmetics.Remove(cosmetic);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < cosmetic.SpriteLayerGroups.Length; i++)
            selfCCGData.layersCosmetics[cosmetic.SpriteLayerGroups[i].Layer].Remove(cosmetic);

        if (cosmetic is IDynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic is UpdatableAndDeletable dynamicCosmeticUpdatable)
        {
            dynamicCosmeticUpdatable.RemoveFromRoom();
        }
    }

    internal static void AddDynamicCreatureCosmeticsToDrawableObjects(this GraphicsModule self)
    {
        if (self.owner.room == null)
        {
            return;
        }

        self.RemoveDynamicCreatureCosmeticsFromDrawableObjects();

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

    internal static void RemoveDynamicCreatureCosmeticsFromDrawableObjects(this GraphicsModule self)
    {
        if (self.owner.room == null)
        {
            return;
        }

        Plugin.LogCCGDebug("Removing cosmetics from drawable objects");

        var wearerCCGData = self.GetGraphicsModuleCCGData();

        for (int i = 0; i < wearerCCGData.cosmetics.Count; i++)
        {
            if (wearerCCGData.cosmetics[i].SLeaser != null)
            {
                wearerCCGData.cosmetics[i].SLeaser.CleanSpritesAndRemove();
            }

            if (wearerCCGData.cosmetics[i] is IDrawable cosmeticDrawable)
            {
                self.owner.room.drawableObjects.Remove(cosmeticDrawable);
            }
        }
    }

    //
    // Internal stuff
    //

    public static void ReorderDynamicCosmetics(this GraphicsModule graphicsModule)
    {
        var graphicsModuleCCGData = graphicsModule.GetGraphicsModuleCCGData();

        Plugin.LogDebug($"//");
        Plugin.LogDebug($"//-- Reordering Dynamic Cosmetics.");
        Plugin.LogDebug($"//");

        for (int i = 0; i < graphicsModuleCCGData.cosmetics.Count; i++)
        {
            if (graphicsModuleCCGData.cosmetics[i] is IDynamicCreatureCosmetic dynamicCosmetic)
            {
                dynamicCosmetic.SetLayerGroupsNeedsReorder(true);
            }
        }


        for (int layerIndex = 0; layerIndex < graphicsModuleCCGData.layersCosmetics.Count; layerIndex++)
        {
            var currentLayerCosmetics = graphicsModuleCCGData.layersCosmetics[layerIndex];
            for (int j = 0; j < currentLayerCosmetics.Count; j++)
            {
                if (currentLayerCosmetics[j] is IDynamicCreatureCosmetic dynamicCosmetic)
                    dynamicCosmetic.ReorderSpritesInLayerGroup(layerIndex);
            }
        }

        Plugin.LogDebug($"//");
        Plugin.LogDebug($"//-- Reordering Finished.");
        Plugin.LogDebug($"//");
    }

    public static string GetSymmetricalAngleFromAsymmetrical(in string angle)
    {
        if (angle.StartsWith("-"))
        {
            return angle.Remove(0, 1);
        }
        return angle;
    }
}