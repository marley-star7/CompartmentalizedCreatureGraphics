using UnityEngine;
using RWCustom;

using System.Runtime.CompilerServices;

namespace CompartmentalizedCreatureGraphics;

public class GraphicsModuleCCGData
{
    public RoomCamera.SpriteLeaser sLeaser;

    /// <summary>
    /// Enable this to use the updated graphics.
    /// </summary>
    public bool compartmentalizedGraphicsEnabled = true;

    public List<DynamicCosmetic> dynamicCosmetics = new();

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

    public static void AddDynamicCosmetic(this GraphicsModule graphicsModule, DynamicCosmetic cosmetic)
    {
        cosmetic.Equip(graphicsModule.owner as Creature);
    }
}