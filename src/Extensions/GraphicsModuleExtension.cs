namespace CompartmentalizedCreatureGraphics.Extensions;

public class GraphicsModuleCCGData
{
    public RoomCamera.SpriteLeaser? sLeaser;

    /// <summary>
    /// Enable this to use the updated graphics.
    /// </summary>
    public bool compartmentalizedGraphicsEnabled = false;

    //-- MR7: TODO: could swap these lists with swapback arrays for better performance if necessary down the line.

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

        for (int i = 0; i < graphicsModuleCCGData.cosmetics.Count; i++)
        {
            if (graphicsModuleCCGData.cosmetics[i] is not IDynamicCreatureCosmetic dynamicCosmetic)
                continue;

            dynamicCosmetic.AddToContainer(sLeaser, rCam, newContainer);
            dynamicCosmetic.SetLayerGroupsNeedsReorder(true);
        }

        Plugin.LogDebug($"Adding {graphicsModuleCCGData.cosmetics.Count} dynamic cosmetics to container.");

        // We go by the layersCosmetics list, which contains all cosmetics in the order they should be rendered.
        // Traveling bottom up.

        for (int layerIndex = 0; layerIndex < graphicsModuleCCGData.layersCosmetics.Count; layerIndex++)
        {
            var currentLayerCosmetics = graphicsModuleCCGData.layersCosmetics[layerIndex];
            for (int j = 0; j < currentLayerCosmetics.Count; j++)
            {
                if (currentLayerCosmetics[j] is not IDynamicCreatureCosmetic dynamicCosmetic)
                    continue;

                //-- MR7: TODO: RENAME THESE FUNCTIONS OH MY GAWDD
                dynamicCosmetic.ReorderSpritesInLayerGroup(layerIndex);
                dynamicCosmetic.SetLayerGroupNeedsReorder(layerIndex, false);
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