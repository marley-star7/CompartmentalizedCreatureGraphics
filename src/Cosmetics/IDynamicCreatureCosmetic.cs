using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CompartmentalizedCreatureGraphics.CCGEnums;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Functionality for cosmetics that can be dynamically equipped/unequipped to a creature.
/// </summary>
public interface IDynamicCreatureCosmetic : ICreatureCosmetic, IDrawable
{
    public Creature wearer { get; }
    public GraphicsModule? wearerGraphics { get; }
}

public static class IDynamicCreatureCosmeticExtension
{
    public static bool IsUnequipped(this IDynamicCreatureCosmetic cosmetic)
    {
        return cosmetic.wearer == null;
    }

    //
    // Layer Stuff
    //

    public static void SetLayerGroupNeedsReorder(this IDynamicCreatureCosmetic cosmetic, int layer, bool value)
    {
        cosmetic.spriteLayerGroups[cosmetic.GetLayerGroupIndexForLayer(layer)].needsReorder = value;
    }

    public static void SetLayerGroupsNeedsReorder(this IDynamicCreatureCosmetic cosmetic, bool value)
    {
        for (int i = 0; i < cosmetic.spriteLayerGroups.Length; i++)
        {
            cosmetic.spriteLayerGroups[i].needsReorder = value;
        }
    }

    public static void RemoveFromContainer(this IDynamicCreatureCosmetic cosmetic)
    {
        for (int i = 0; i < cosmetic.sLeaser.sprites.Length; i++)
        {
            cosmetic.sLeaser.sprites[i].RemoveFromContainer();
        }
    }

    private static void OrderSpritesInFrontOtherCosmeticInLayerGroup(this IDynamicCreatureCosmetic cosmetic, SpriteLayerGroup layerGroup, ICreatureCosmetic? referenceCosmetic, SpriteLayerGroup referenceCosmeticLayerGroup)
    {
        //-- MR7: Behold, my wall of way too many checks because I was frustrated at some error lol.
        if (cosmetic.sLeaser == null)
        {
            Plugin.LogError($"OrderSpritesInFrontOtherCosmeticInLayerGroup failed!, this cosmetic sprite leaser is null!");
            return;
        }
        if (referenceCosmetic == null)
        {
            Plugin.LogError($"OrderSpritesInFrontOtherCosmeticInLayerGroup failed!, referenceCosmetic is null!");
            return;
        }
        if (referenceCosmetic.sLeaser == null)
        {
            Plugin.LogError($"OrderSpritesInFrontOtherCosmeticInLayerGroup failed!, refrence cosmetic sprite leaser is null!");
            return;
        }

        try
        {
            var referenceSprite = referenceCosmetic.sLeaser.sprites[referenceCosmeticLayerGroup.endSpriteIndex];

            // Position first sprite in front of reference
            var startSprite = cosmetic.sLeaser.sprites[layerGroup.startSpriteIndex];

            if (startSprite.element != null && referenceSprite.element != null)
                Plugin.LogDebug($"! Sprite of: {startSprite.element.name} to position in front of sprite {referenceSprite.element.name} in layer: {referenceCosmeticLayerGroup.layer} in container {referenceSprite.container.depth}");

            startSprite.MoveInFrontOfOtherNodeAndInContainer(referenceSprite);

            // Position remaining sprites in sequence
            for (int i = layerGroup.startSpriteIndex + 1; i < layerGroup.endSpriteIndex + 1; i++)
            {
                var currentSprite = cosmetic.sLeaser.sprites[i];
                var lastSprite = cosmetic.sLeaser.sprites[i - 1];

                if (currentSprite.element != null && lastSprite.element != null)
                    Plugin.LogDebug($"! Sprite of: {currentSprite.element.name} to position in front of sprite {lastSprite.element.name}");

                currentSprite.MoveInFrontOfOtherNodeAndInContainer(lastSprite);
            }
        }
        catch (Exception e)
        {
            Plugin.LogError(e.ToString());
        }
    }

    public static void ReorderSpritesInLayerGroup(this IDynamicCreatureCosmetic cosmetic, int layer)
    {
        if (cosmetic.wearerGraphics == null)
        {
            Plugin.LogError($"Cannot update render order - wearer or graphics module is null!");
            return;
        }
        if (cosmetic.sLeaser == null)
        {
            Plugin.LogError($"Cannot update render order - sprite leaser is null!");
            return;
        }

        var wearerGraphicsCCGData = cosmetic.wearerGraphics.GetGraphicsModuleCCGData();
        var layerGroup = cosmetic.spriteLayerGroups[cosmetic.GetLayerGroupIndexForLayer(layer)];

        // Find the closest cosmetic in this or lower layers to position in front of
        // Search from starting layer down to 0
        for (int refLayer = layerGroup.layer; refLayer >= 0; refLayer--)
        {
            var refLayerCosmetics = wearerGraphicsCCGData.layersCosmetics[refLayer];

            // Search backwards through cosmetics in this layer
            for (int candidateIndex = refLayerCosmetics.Count - 1; candidateIndex >= 0; candidateIndex--)
            {
                var candidate = refLayerCosmetics[candidateIndex];

                // Do not reference any cosmetics that are also not properly ordered yet.
                if (candidate is IDynamicCreatureCosmetic dynamicCandidate && dynamicCandidate.spriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].needsReorder)
                {
                    if (dynamicCandidate.sLeaser == null)
                        Plugin.LogDebug($"Checking candidate: {dynamicCandidate} Sleaser is null");
                    else
                        Plugin.LogDebug($"Checking candidate: {dynamicCandidate.sLeaser.sprites[dynamicCandidate.spriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].startSpriteIndex].element.name} in layer {refLayer}, not ordered yet, skipping...");
                    
                    continue;
                }

                if (candidate == null)
                    Plugin.LogError($"No reference cosmetic found for layer {layerGroup.layer}");
                else
                {
                    SpriteLayerGroup candidateCosmeticLayerGroup = candidate.spriteLayerGroups[candidate.GetLayerGroupIndexForLayer(refLayer)];
                    cosmetic.OrderSpritesInFrontOtherCosmeticInLayerGroup(layerGroup, candidate, candidateCosmeticLayerGroup);
                    cosmetic.SetLayerGroupNeedsReorder(layer, false);
                    return;
                }
            }
        }

        cosmetic.SetLayerGroupNeedsReorder(layer, false);
    }
}
