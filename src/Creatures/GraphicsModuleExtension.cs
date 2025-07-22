using UnityEngine;
using RWCustom;

using System.Runtime.CompilerServices;
using UnityEngine.PlayerLoop;
using CompartmentalizedCreatureGraphics.Extensions;

namespace CompartmentalizedCreatureGraphics;

public class GraphicsModuleCCGData
{
    public RoomCamera.SpriteLeaser? sLeaser;

    /// <summary>
    /// Enable this to use the updated graphics.
    /// </summary>
    public bool compartmentalizedGraphicsEnabled = true;

    //-- MR7: TODO: could swap these lists with swapback arrays for better performance if necessary down the line.

    public List<ICosmetic> cosmetics = new();
    /// <summary>
    /// Dictionary containing refrences to dynamic cosmetics rendering in a certain layer.
    /// The int key should be the cosmeticLayer Enum of which cosmetic layer we are on.
    /// The value inside refrences a dynamic cosmetic which is using this layer.
    /// </summary>
    public Dictionary<int, List<ICosmetic>> layersCosmetics = new();

    public WeakReference<GraphicsModule> graphicsModuleRef;

    public GraphicsModuleCCGData(GraphicsModule graphicsModuleRef)
    {
        this.graphicsModuleRef = new WeakReference<GraphicsModule>(graphicsModuleRef);
    }
}

public static class GraphicsModuleCraftingExtension
{
    internal static readonly ConditionalWeakTable<GraphicsModule, GraphicsModuleCCGData> craftingDataConditionalWeakTable = new();

    public static GraphicsModuleCCGData GetGraphicsModuleCCGData(this GraphicsModule graphicsModule)
    {
        return craftingDataConditionalWeakTable.GetValue(graphicsModule, _ => new GraphicsModuleCCGData(graphicsModule));
    }

    internal static void AddDynamicCosmeticsToContainer(this GraphicsModule graphicsModule, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        var graphicsModuleCCGData = graphicsModule.GetGraphicsModuleCCGData();

        Plugin.Logger.LogDebug($"Adding {graphicsModuleCCGData.cosmetics.Count} dynamic cosmetics to container.");

        for (int i = 0; i < graphicsModuleCCGData.cosmetics.Count; i++)
        {
            if (graphicsModuleCCGData.cosmetics[i] is DynamicCosmetic dynamicCosmetic)
                dynamicCosmetic.SetSpriteLayersNeedReorder(true);
        }

        // We go by the layersCosmetics list, which contains all cosmetics in the order they should be rendered.
        for (int layer = 0; layer < graphicsModuleCCGData.layersCosmetics.Count; layer++)
        {
            for (int j = 0; j < graphicsModuleCCGData.layersCosmetics[layer].Count; j++)
            {
                if (graphicsModuleCCGData.layersCosmetics[layer][j] is DynamicCosmetic dynamicCosmetic)
                {
                    dynamicCosmetic.AddToContainer(sLeaser, rCam, newContainer);
                    dynamicCosmetic.SetSpriteLayerNeedsReorder(layer, false);
                }
            }
        }
    }

    /// <summary>
    /// Runs through all dynamic cosmetics in the graphics module and updates their sprite render order.
    /// </summary>
    /// <param name="graphicsModule"></param>
    public static void UpdateDynamicCosmeticsSpriteRenderOrder(this GraphicsModule graphicsModule)
    {
        var graphicsModuleCCGData = graphicsModule.GetGraphicsModuleCCGData();

        // Loop through back to front of cosmetics in layer order.
        for (int i = 0; i < graphicsModuleCCGData.layersCosmetics.Count; i++)
        {
            for (int j = 0; j < graphicsModuleCCGData.layersCosmetics[i].Count; j++)
            {
                //-- MR7 TODO: this current logic often causes multiple render order re-updating, which could likely be optimized better.
                // Likely through a function that allows you to update the render order of a specific layer.

                // Only add to container if is dynamic cosmetic, since they are the ones that need it.
                if (graphicsModuleCCGData.layersCosmetics[i][j] is DynamicCosmetic)
                {
                    var dynamicCosmetic = (DynamicCosmetic)graphicsModuleCCGData.layersCosmetics[i][j];
                    dynamicCosmetic.ReorderSpriteLayers();
                }
            }
        }
    }

    // TODO: need to set up system for normal cosmetics too, so that they are stored in the layers,
    // Then can check easily for the fnode position in this loop that would correspond to in front of base head for example.
    /*
    public static FNode GetFrontFNodeInLayer(this GraphicsModule graphicsModule, int layer)
    {
        var ccgData = graphicsModule.GetGraphicsModuleCCGData();
        // If we have no cosmetics in this layer, return the most relevant fNode instead.
        if (ccgData.cosmeticLayersDynamicCosmetics.Count > 0)
        {
            var firstCosmeticInLayer = ccgData.cosmeticLayersDynamicCosmetics[layer].First();
            return firstCosmeticInLayer.FirstSpriteInSpritesLayer(layer);
        }
        else
        {
            for (int i = layer; i <= 0; i-- )
        }
    }

    public static FNode GetBackFNodeInLayer(this GraphicsModule graphicsModule, int layer)
    {
        var ccgData = graphicsModule.GetGraphicsModuleCCGData();
        var lastCosmeticInLayer = ccgData.cosmeticLayersDynamicCosmetics[layer].Last();
        return lastCosmeticInLayer.LastSpriteInSpritesLayer(layer);
    }
    */
}