using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CompartmentalizedCreatureGraphics.Enums;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Functionality for cosmetics that can be dynamically equipped/unequipped to a creature.
/// </summary>
public interface IDynamicCreatureCosmetic : ICreatureCosmetic, IDrawable
{
    public Creature Wearer { get; }
    public GraphicsModule? WearerGraphics { get; }
}

public static class IDynamicCreatureCosmeticExtension
{
    public static bool IsUnequipped(this IDynamicCreatureCosmetic cosmetic)
    {
        return cosmetic.Wearer == null;
    }

    //
    // Layer Stuff
    //

    public static void SetLayerGroupNeedsReorder(this IDynamicCreatureCosmetic cosmetic, int layer, bool value)
    {
        cosmetic.SpriteLayerGroups[cosmetic.GetLayerGroupIndexForLayer(layer)].needsReorder = value;
    }

    public static void SetLayerGroupsNeedsReorder(this IDynamicCreatureCosmetic cosmetic, bool value)
    {
        for (int i = 0; i < cosmetic.SpriteLayerGroups.Length; i++)
        {
            cosmetic.SpriteLayerGroups[i].needsReorder = value;
        }
    }

    public static void RemoveFromContainer(this IDynamicCreatureCosmetic cosmetic)
    {
        for (int i = 0; i < cosmetic.SLeaser.sprites.Length; i++)
        {
            cosmetic.SLeaser.sprites[i].RemoveFromContainer();
        }
    }

    private static void OrderSpritesInFrontOtherCosmeticInLayerGroup(this IDynamicCreatureCosmetic cosmetic, SpriteLayerGroup layerGroup, ICreatureCosmetic? referenceCosmetic, SpriteLayerGroup referenceCosmeticLayerGroup)
    {
        //-- MS7: Behold, my wall of way too many checks because I was frustrated at some error lol.
        if (cosmetic.SLeaser == null)
        {
            Plugin.LogCCGError($"OrderSpritesInFrontOtherCosmeticInLayerGroup failed!, this cosmetic sprite leaser is null!");
            return;
        }
        if (referenceCosmetic == null)
        {
            Plugin.LogCCGError($"OrderSpritesInFrontOtherCosmeticInLayerGroup failed!, referenceCosmetic is null!");
            return;
        }
        if (referenceCosmetic.SLeaser == null)
        {
            Plugin.LogCCGError($"OrderSpritesInFrontOtherCosmeticInLayerGroup failed!, refrence cosmetic sprite leaser is null!");
            return;
        }

        try
        {
            var referenceSprite = referenceCosmetic.SLeaser.sprites[referenceCosmeticLayerGroup.EndSpriteIndex];

            // Position first sprite in front of reference
            var startSprite = cosmetic.SLeaser.sprites[layerGroup.StartSpriteIndex];

            if (startSprite.element != null && referenceSprite.element != null)
                Plugin.LogCCGDebug($"! Sprite of: {startSprite.element.name} to position in front of sprite {referenceSprite.element.name} in layer: {referenceCosmeticLayerGroup.Layer} in container {referenceSprite.container.depth}");

            startSprite.MoveInFrontOfOtherNodeAndInContainer(referenceSprite);

            // Position remaining sprites in sequence
            for (int i = layerGroup.StartSpriteIndex + 1; i < layerGroup.EndSpriteIndex + 1; i++)
            {
                var currentSprite = cosmetic.SLeaser.sprites[i];
                var lastSprite = cosmetic.SLeaser.sprites[i - 1];

                if (currentSprite.element != null && lastSprite.element != null)
                    Plugin.LogCCGDebug($"! Sprite of: {currentSprite.element.name} to position in front of sprite {lastSprite.element.name}");

                currentSprite.MoveInFrontOfOtherNodeAndInContainer(lastSprite);
            }
        }
        catch (Exception e)
        {
            Plugin.LogCCGError(e.ToString());
        }
    }

    public static void ReorderSpritesInLayerGroup(this IDynamicCreatureCosmetic cosmetic, int layer)
    {
        if (cosmetic.WearerGraphics == null)
        {
            Plugin.LogCCGError($"Cannot update render order - wearer or graphics module is null!");
            return;
        }
        if (cosmetic.SLeaser == null)
        {
            Plugin.LogCCGError($"Cannot update render order - sprite leaser is null!");
            return;
        }

        var wearerGraphicsCCGData = cosmetic.WearerGraphics.GetGraphicsModuleCCGData();
        var layerGroup = cosmetic.SpriteLayerGroups[cosmetic.GetLayerGroupIndexForLayer(layer)];

        // Find the closest cosmetic in this or lower layers to position in front of
        // Search from starting layer down to 0
        for (int refLayer = layerGroup.Layer; refLayer >= 0; refLayer--)
        {
            var refLayerCosmetics = wearerGraphicsCCGData.layersCosmetics[refLayer];

            // Search backwards through cosmetics in this layer
            for (int candidateIndex = refLayerCosmetics.Count - 1; candidateIndex >= 0; candidateIndex--)
            {
                var candidate = refLayerCosmetics[candidateIndex];

                // Do not reference any cosmetics that are also not properly ordered yet.
                if (candidate is IDynamicCreatureCosmetic dynamicCandidate && dynamicCandidate.SpriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].needsReorder)
                {
                    if (dynamicCandidate.SLeaser == null)
                        Plugin.LogCCGDebug($"Checking candidate: {dynamicCandidate} Sleaser is null");
                    else
                        Plugin.LogCCGDebug($"Checking candidate: {dynamicCandidate.SLeaser.sprites[dynamicCandidate.SpriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].StartSpriteIndex].element.name} in layer {refLayer}, not ordered yet, skipping...");
                    
                    continue;
                }

                if (candidate == null)
                    Plugin.LogCCGError($"No reference cosmetic found for layer {layerGroup.Layer}");
                else
                {
                    SpriteLayerGroup candidateCosmeticLayerGroup = candidate.SpriteLayerGroups[candidate.GetLayerGroupIndexForLayer(refLayer)];
                    cosmetic.OrderSpritesInFrontOtherCosmeticInLayerGroup(layerGroup, candidate, candidateCosmeticLayerGroup);
                    cosmetic.SetLayerGroupNeedsReorder(layer, false);
                    return;
                }
            }
        }

        cosmetic.SetLayerGroupNeedsReorder(layer, false);
    }
}
