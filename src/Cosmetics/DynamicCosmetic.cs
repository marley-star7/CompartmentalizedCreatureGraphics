using CompartmentalizedCreatureGraphics.Cosmetics;
using System.Xml.Linq;

namespace CompartmentalizedCreatureGraphics;

/// <summary>
/// DynamicCosmetics are cosmetics able to be equipped and unequipped on demand.
/// </summary>
public class DynamicCosmetic : UpdatableAndDeletable, ICosmetic, IDrawable
{
    public Creature? wearer = null;

    public bool isEquipped => wearer != null && wearer.graphicsModule.GetGraphicsModuleCCGData().cosmetics.Contains(this);

    protected RoomCamera.SpriteLeaser sLeaser;

    public RoomCamera.SpriteLeaser SpriteLeaser
    {
        get => sLeaser;
    }

    protected Dictionary<int, SpriteLayer> _spriteLayers;
    public Dictionary<int, SpriteLayer> SpriteLayers
    {
        get => _spriteLayers;
        set { _spriteLayers = value; }
    }

    public SpriteEffectLayer[] spriteEffectLayers;

    public FSprite LastSprite
    {
        get => sLeaser.sprites[sLeaser.sprites.Length - 1];
    }

    public DynamicCosmetic(Dictionary<int, SpriteLayer> spriteLayers)
    {
        this._spriteLayers = spriteLayers;
    }

    public virtual void Equip(Creature wearer)
    {
        this.wearer = wearer;
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Add(this);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        foreach(KeyValuePair<int, SpriteLayer> kvp in SpriteLayers)
            wearerCCGData.layersCosmetics[kvp.Key].Add(this);

        if (this.wearer.room != null)
            wearer.room.AddObject(this);
    }

    public virtual void Unequip(Creature wearer)
    {
        this.wearer = wearer;
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Remove(this);
        // Properly remove this cosmetics sprite layers information to the wearer graphics module data.
        foreach (KeyValuePair<int, SpriteLayer> kvp in SpriteLayers)
            wearerCCGData.layersCosmetics[kvp.Key].Remove(this);


        this.Destroy();
    }

    //
    // WEARER IDRAWABLES
    //

    public virtual void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {

    }

    //-- MR7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    public virtual void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {

    }

    //
    // COLLISION
    //

    public virtual void OnWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk)
    {

    }

    public virtual void OnWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact)
    {

    }

    //
    // BASICALLY UNUSED IN FAVOR OF ONWEARER
    //

    public virtual void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        this.sLeaser = sLeaser;
    }

    public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        this.sLeaser = sLeaser;
    }

    public virtual void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {

    }

    public void SetSpriteLayersNeedReorder(bool value)
    {
        // Create a copy of the SpriteLayer struct, modify it, and then update the dictionary
        foreach (var item in _spriteLayers.ToList())
        {
            var updatedLayer = item.Value;
            updatedLayer.needsReorder = value;
            _spriteLayers[item.Key] = updatedLayer;
        }
    }

    public void ReorderSpriteLayers()
    {
        if (wearer?.graphicsModule == null)
        {
            Plugin.Logger.LogError("Cannot update render order - wearer or graphics module is null");
            return;
        }

        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();
        if (wearerCCGData.sLeaser == null)
        {
            Plugin.Logger.LogError("Cannot update render order - wearer's sprite Leaser is null");
            return;
        }

        if (SpriteLayers == null || SpriteLayers.Count == 0)
        {
            Plugin.Logger.LogWarning("No sprite layers defined for this cosmetic");
            return;
        }

        // TODO: i gotta go back the array....
        foreach (KeyValuePair<int, SpriteLayer> layer in SpriteLayers)
        {
            // Find the closest cosmetic in this or lower layers to position in front of
            ICosmetic? referenceCosmetic = FindReferenceCosmetic(wearerCCGData, layer.Key);

            if (referenceCosmetic == null)
            {
                Plugin.Logger.LogError($"No reference cosmetic found for layer {layer}");
                continue;
            }

            PositionSpritesRelativeToReference(layer.Value, referenceCosmetic);
        }
    }

    private ICosmetic? FindReferenceCosmetic(GraphicsModuleCCGData wearerCCGData, int cosmeticsLayer)
    {
        // Search from starting layer down to 0
        for (int refLayer = cosmeticsLayer; refLayer >= 0; refLayer--)
        {
            var layerCosmetics = wearerCCGData.layersCosmetics[refLayer];
            if (layerCosmetics == null) continue;

            // Search backwards through cosmetics in this layer
            for (int i = layerCosmetics.Count - 1; i >= 0; i--)
            {
                var candidate = layerCosmetics[i];

                // Skip if candidate not in it's correct order either.
                if (candidate is DynamicCosmetic dynamicCandidate && dynamicCandidate.SpriteLayers[refLayer].needsReorder)
                    continue;

                return candidate;
            }
        }

        return null;
    }

    /*
    private FContainer? GetSpriteContainer(int spriteIndex)
    {
        return sLeaser.sprites[spriteIndex]._container;
    }

    private bool IsCosmeticLayerInSameContainer(ICosmetic otherCosmetic, int ourLayer, int theirLayer)
    {
        var ourLayerInfo = SpriteLayers[ourLayer];
        var ourContainer = GetSpriteContainer(ourLayerInfo.firstSpriteIndex);

        if (otherCosmetic is DynamicCosmetic dynamicCosmetic)
        {
            var theirLayerInfo = dynamicCosmetic.SpriteLayers[theirLayer];
            var theirContainer = dynamicCosmetic.GetSpriteContainer(theirLayerInfo.firstSpriteIndex);

            return theirContainer == ourContainer;
        }

        //TODO: If the other cosmetic is not a DynamicCosmetic, we assume it has only one sprite, which is probably a problem but works for now.
        return otherCosmetic.SpriteLeaser.sprites[0]._container == ourContainer;
    }
    */

    private void PositionSpritesRelativeToReference(SpriteLayer layerInfo, ICosmetic referenceCosmetic)
    {
        var referenceSprites = referenceCosmetic.SpriteLeaser.sprites;
        if (referenceSprites == null || referenceSprites.Length == 0)
        {
            Plugin.Logger.LogError("Reference cosmetic has no sprites");
            return;
        }

        // Get the last sprite of the reference cosmetic
        var referenceSprite = referenceSprites[referenceSprites.Length - 1];

        // Position first sprite in front of reference
        sLeaser.sprites[layerInfo.firstSpriteIndex].MoveInFrontOfOtherNode(referenceSprite);

        // Position remaining sprites in sequence
        for (int i = layerInfo.firstSpriteIndex + 1; i < layerInfo.lastSpriteIndex; i++)
        {
            sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
        }

        Plugin.Logger.LogDebug("sprite of: " + sLeaser.sprites[layerInfo.firstSpriteIndex].element.name + " positioned in front of sprite " + referenceSprite.element.name);
    }

    public virtual void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer? newContainer)
    {
        newContainer ??= rCam.ReturnFContainer("Midground");

        foreach (FSprite fsprite in sLeaser.sprites)
        {
            fsprite.RemoveFromContainer();
            newContainer.AddChild(fsprite);
        }

        if (wearer == null)
        {
            Plugin.Logger.LogError("Cannot add cosmetic to container - wearer is null");
            return;
        }

        ReorderSpriteLayers();
    }

    //
    // UTILITY FUNCTIONS
    //

    public void SetSpriteLayerNeedsReorder(int layer, bool value)
    {
        if (SpriteLayers.TryGetValue(layer, out SpriteLayer spriteLayer))
        {
            spriteLayer.needsReorder = value;
            SpriteLayers[layer] = spriteLayer; // Update the dictionary entry
        }
        else
        {
            Plugin.Logger.LogWarning($"Sprite layer {layer} does not exist in this cosmetic.");
        }
    }

    public FSprite FirstSpriteInSpritesLayer(int layer)
    {
        var firstCosmeticInLayersSpriteIndex = SpriteLayers[layer].firstSpriteIndex;
        return sLeaser.sprites[firstCosmeticInLayersSpriteIndex];
    }

    public FSprite LastSpriteInSpritesLayer(int layer)
    {
        var firstCosmeticInLayersSpriteIndex = SpriteLayers[layer].lastSpriteIndex;
        return sLeaser.sprites[firstCosmeticInLayersSpriteIndex];
    }
}