using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Functionality for cosmetics that can be dynamically equipped/unequipped to a creature.
/// </summary>
public interface IDynamicCreatureCosmetic : ICreatureCosmetic
{
    public Creature Wearer { get; }
    public GraphicsModule? WearerGraphics { get; }

    public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer);
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

    public static void ReorderSpritesInLayerGroup(this IDynamicCreatureCosmetic cosmetic, int layer)
    {
        if (cosmetic.Wearer?.graphicsModule == null)
        {
            Plugin.LogError("Cannot update render order - wearer or graphics module is null");
            return;
        }

        var wearerCCGData = cosmetic.Wearer.graphicsModule.GetGraphicsModuleCCGData();

        var layerGroup = cosmetic.SpriteLayerGroups[cosmetic.GetLayerGroupIndexForLayer(layer)];

        // Find the closest cosmetic in this or lower layers to position in front of
        // Search from starting layer down to 0
        for (int refLayer = layerGroup.layer; refLayer >= 0; refLayer--)
        {
            var refLayerCosmetics = wearerCCGData.layersCosmetics[refLayer];

            // Search backwards through cosmetics in this layer
            for (int candidateIndex = refLayerCosmetics.Count - 1; candidateIndex >= 0; candidateIndex--)
            {
                var candidate = refLayerCosmetics[candidateIndex];

                // Do not reference any cosmetics that are also not properly ordered yet.
                if (candidate is IDynamicCreatureCosmetic dynamicCandidate && dynamicCandidate.SpriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].needsReorder)
                {
                    Plugin.LogDebug($"Checking candidate: {dynamicCandidate.SLeaser.sprites[dynamicCandidate.SpriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].startSpriteIndex].element.name} in layer {refLayer}, not ordered yet, skipping...");
                    continue;
                }

                // Skip if not in the same container
                //if (!IsCosmeticInSameContainerForLayer(candidate, layer))
                //    continue;

                Plugin.LogDebug($"Found candidate: {candidate} in layer {refLayer}");

                if (candidate == null)
                    Plugin.LogError($"No reference cosmetic found for layer {layerGroup.layer}");
                else
                {
                    SpriteLayerGroup candidateCosmeticLayerGroup = candidate.SpriteLayerGroups[candidate.GetLayerGroupIndexForLayer(refLayer)];
                    OrderSpritesInFrontOtherCosmeticInLayerGroup(layerGroup, candidate, candidateCosmeticLayerGroup);
                    return;
                }
            }
        }

        //-- MR7: I just put this as a void function in case I ever want to move it out later.
        void OrderSpritesInFrontOtherCosmeticInLayerGroup(SpriteLayerGroup layerGroup, ICreatureCosmetic referenceCosmetic, SpriteLayerGroup referenceCosmeticLayerGroup)
        {
            var referenceSprite = referenceCosmetic.SLeaser.sprites[referenceCosmeticLayerGroup.endSpriteIndex];

            // Position first sprite in front of reference
            cosmetic.SLeaser.sprites[layerGroup.startSpriteIndex].MoveInFrontOfOtherNode(referenceSprite);
            Plugin.LogDebug("sprite of: " + cosmetic.SLeaser.sprites[layerGroup.startSpriteIndex].element.name + " positioned in front of sprite " + referenceSprite.element.name);

            // Position remaining sprites in sequence
            for (int i = layerGroup.startSpriteIndex + 1; i < layerGroup.endSpriteIndex; i++)
            {
                cosmetic.SLeaser.sprites[i].MoveInFrontOfOtherNode(cosmetic.SLeaser.sprites[i - 1]);
                Plugin.LogDebug("sprite of: " + cosmetic.SLeaser.sprites[i].element.name + " positioned in front of sprite " + cosmetic.SLeaser.sprites[i - 1].element.name);
            }
        }
    }
}
