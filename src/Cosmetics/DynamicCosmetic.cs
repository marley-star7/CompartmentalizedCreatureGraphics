namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// DynamicCosmetics are cosmetics able to be equipped and unequipped on demand.
/// </summary>
public class DynamicCosmetic : UpdatableAndDeletable, ICosmetic, IDrawable
{
    public Creature? wearer = null;

    public bool isEquipped => wearer != null && wearer.graphicsModule.GetGraphicsModuleCCGData().cosmetics.Contains(this);

    protected RoomCamera.SpriteLeaser? sLeaser;

    public RoomCamera.SpriteLeaser? SpriteLeaser
    {
        get => sLeaser;
    }

    public SpriteLayerGroup[] _spriteLayerGroups;
    public SpriteLayerGroup[] SpriteLayerGroups
    {
        get => _spriteLayerGroups;
        set
        {
            _spriteLayerGroups = value;
        }
    }
    public SpriteEffectGroup[] spriteEffectGroups;

    public FSprite LastSprite
    {
        get => sLeaser.sprites[sLeaser.sprites.Length - 1];
    }

    public DynamicCosmetic(SpriteLayerGroup[] spriteLayers)
    {
        this._spriteLayerGroups = spriteLayers;
        this.spriteEffectGroups = new SpriteEffectGroup[0];
    }

    public virtual void Equip(Creature wearer)
    {
        this.wearer = wearer;
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Add(this);
        // Add this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < SpriteLayerGroups.Length; i++)
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].layer].Add(this);

        if (this.wearer.room != null)
            wearer.room.AddObject(this);
    }

    public virtual void Unequip(Creature wearer)
    {
        this.wearer = wearer;
        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        wearerCCGData.cosmetics.Remove(this);
        // Properly remove this cosmetics sprite layers information to the wearer graphics module data.
        for (int i = 0; i < SpriteLayerGroups.Length; i++)
            wearerCCGData.layersCosmetics[SpriteLayerGroups[i].layer].Remove(this);


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

    public void ReorderSpritesInLayerGroup(int layer)
    {
        if (wearer?.graphicsModule == null)
        {
            Plugin.Logger.LogError("Cannot update render order - wearer or graphics module is null");
            return;
        }

        var wearerCCGData = wearer.graphicsModule.GetGraphicsModuleCCGData();

        var layerGroup = SpriteLayerGroups[this.GetLayerGroupIndexForLayer(layer)];

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
                if (candidate is DynamicCosmetic dynamicCandidate && dynamicCandidate.SpriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].needsReorder)
                {
                    Plugin.DebugLog($"Checking candidate: {dynamicCandidate.SpriteLeaser.sprites[dynamicCandidate.SpriteLayerGroups[dynamicCandidate.GetLayerGroupIndexForLayer(refLayer)].firstSpriteIndex].element.name} in layer {refLayer}, not ordered yet, skipping...");
                    continue;
                }

                // Skip if not in the same container
                //if (!IsCosmeticInSameContainerForLayer(candidate, layer))
                //    continue;

                Plugin.DebugLog($"Found candidate: {candidate} in layer {refLayer}");

                if (candidate == null)
                    Plugin.Logger.LogError($"No reference cosmetic found for layer {layerGroup.layer}");
                else
                {
                    SpriteLayerGroup candidateCosmeticLayerGroup = candidate.SpriteLayerGroups[candidate.GetLayerGroupIndexForLayer(refLayer)];
                    OrderSpritesInFrontOtherCosmeticInLayerGroup(layerGroup, candidate, candidateCosmeticLayerGroup);
                    return;
                }
            }
        }
    }
    private void OrderSpritesInFrontOtherCosmeticInLayerGroup(SpriteLayerGroup layerGroup, ICosmetic referenceCosmetic, SpriteLayerGroup referenceCosmeticLayerGroup)
    {
        var referenceSprite = referenceCosmetic.SpriteLeaser.sprites[referenceCosmeticLayerGroup.lastSpriteIndex];

        // Position first sprite in front of reference
        sLeaser.sprites[layerGroup.firstSpriteIndex].MoveInFrontOfOtherNode(referenceSprite);
        Plugin.DebugLog("sprite of: " + sLeaser.sprites[layerGroup.firstSpriteIndex].element.name + " positioned in front of sprite " + referenceSprite.element.name);

        // Position remaining sprites in sequence
        for (int i = layerGroup.firstSpriteIndex + 1; i < layerGroup.lastSpriteIndex; i++)
        {
            sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
            Plugin.DebugLog("sprite of: " + sLeaser.sprites[i].element.name + " positioned in front of sprite " + sLeaser.sprites[i - 1].element.name);
        }
    }

    public virtual void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
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
    }

    //
    // UTILITY FUNCTIONS
    //
}