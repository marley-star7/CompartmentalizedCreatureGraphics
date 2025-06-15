using UnityEngine;
using RWCustom;

using System.Runtime.CompilerServices;

namespace CompartmentalizedCreatureGraphics;

public class GraphicsModuleCCGData
{
    // Sprite leaser is shared so we can refrence it from other classes.
    public RoomCamera.SpriteLeaser? sLeaser;
    public List<Cosmetic> cosmetics = new();

    public WeakReference<GraphicsModule> graphicsModuleRef;

    public GraphicsModuleCCGData(GraphicsModule graphicsModuleRef)
    {
        this.graphicsModuleRef = new WeakReference<GraphicsModule>(graphicsModuleRef);
    }
}

public static class GraphicsModuleCraftingExtension
{
    internal static readonly ConditionalWeakTable<GraphicsModule, GraphicsModuleCCGData> craftingDataConditionalWeakTable = new();

    public static GraphicsModuleCCGData GetGraphicsModuleCraftingData(this GraphicsModule graphicsModule) => craftingDataConditionalWeakTable.GetValue(graphicsModule, _ => new GraphicsModuleCCGData(graphicsModule));

    public static void EquipCosmetic(this GraphicsModule graphicsModule, Cosmetic cosmetic)
    {
        cosmetic.Equip(graphicsModule.owner as Creature);
    }
}