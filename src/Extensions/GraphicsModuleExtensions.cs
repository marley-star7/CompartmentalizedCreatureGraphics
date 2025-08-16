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
        for (int i = 0; i < cosmetic.spriteLayerGroups.Length; i++)
            selfCCGData.layersCosmetics[cosmetic.spriteLayerGroups[i].layer].Add(cosmetic);

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
        for (int i = 0; i < cosmetic.spriteLayerGroups.Length; i++)
            selfCCGData.layersCosmetics[cosmetic.spriteLayerGroups[i].layer].Remove(cosmetic);

        if (cosmetic is IDynamicCreatureCosmetic dynamicCosmetic && dynamicCosmetic is UpdatableAndDeletable dynamicCosmeticUpdatable)
        {
            dynamicCosmeticUpdatable.RemoveFromRoom();
        }
    }

    public static void AddDynamicCreatureCosmeticsToRoom(this GraphicsModule self)
    {
        if (self.owner.room == null)
            return;

        var wearerCCGData = self.GetGraphicsModuleCCGData();

        for (int i = 0; i < wearerCCGData.cosmetics.Count; i++)
        {
            if (wearerCCGData.cosmetics[i] is UpdatableAndDeletable cosmeticUpdatable 
                && cosmeticUpdatable.room != self.owner.room)
            {
                cosmeticUpdatable.RemoveFromRoom();
                self.owner.room.AddObject(cosmeticUpdatable);
            }
        }
    }

    public static void RemoveDynamicCreatureCosmeticsFromRoom(this GraphicsModule self)
    {
        if (self.owner.room == null)
            return;

        var wearerCCGData = self.GetGraphicsModuleCCGData();

        for (int i = 0; i < wearerCCGData.cosmetics.Count; i++)
        {
            if (wearerCCGData.cosmetics[i] is UpdatableAndDeletable cosmeticUpdatable)
                cosmeticUpdatable.RemoveFromRoom();
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

    internal static void AddDynamicCosmeticsToContainer(this GraphicsModule graphicsModule, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        var graphicsModuleCCGData = graphicsModule.GetGraphicsModuleCCGData();
        Plugin.LogDebug($"Adding {graphicsModuleCCGData.cosmetics.Count} dynamic cosmetics to container.");

        graphicsModule.ReorderDynamicCosmetics();
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