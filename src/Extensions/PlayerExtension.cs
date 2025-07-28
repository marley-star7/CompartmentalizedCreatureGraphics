using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Extensions;

public static class PlayerExtension
{
    /// <summary>
    /// Creates a cosmetic that simply holds the information of the base player graphics sprites in the cosmetics system.
    /// This is so that we can place cosmetics in front and behind the base player graphics sprites, and also so that we can easily access the base player graphics sprites in the cosmetics system.
    /// </summary>
    /// <param name="playerGraphics"></param>
    /// <returns></returns>
    internal static OriginalCreatureGraphicsCosmeticReference AddOriginalPlayerGraphicsCosmeticReference(this Player player)
    {
        // The Magic Sheet of Sprite Bull-Sheet
        /* 
        Sprite 0 = BodyA
        Sprite 1 = HipsA
        Sprite 2 = Tail
        Sprite 3 = HeadA || B
        Sprite 4 = LegsA
        Sprite 5 = Arm
        Sprite 6 = Arm
        Sprite 7 = TerrainHand
        sprite 8 = TerrainHand
        sprite 9 = FaceA
        sprite 10 = Futile_White with shader Flatlight
        sprite 11 = pixel Mark of comunication
        */

        return new OriginalCreatureGraphicsCosmeticReference(player,
            new SpriteLayerGroup[]
            {
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseBody, 0),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseHips, 1),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseTail, 2),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseHead, 3),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseLegs, 4),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseLeftArm, 5),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseRightArm, 6),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseLeftTerrainHand, 7),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseRightTerrainHand, 8),
                new SpriteLayerGroup((int)CCGEnums.SlugcatCosmeticLayer.BaseFace, 9),
            }
        );
    }

    public static void EquipDynamicSlugcatCosmetic(this Player player, string cosmeticTypeID, string propertiesID)
    {
        var properties = CosmeticManager.GetLoadedDynamicCosmeticProperties(cosmeticTypeID, propertiesID);
        var cosmeticType = CosmeticManager.GetCosmeticTypeFromCosmeticTypeID(cosmeticTypeID);

        IDynamicCreatureCosmetic creatureCosmetic = (IDynamicCreatureCosmetic)Activator.CreateInstance(cosmeticType, player, properties);
        EquipDynamicSlugcatCosmetic(player, creatureCosmetic);
    }

    public static void EquipDynamicSlugcatCosmetic(this Player player, IDynamicCreatureCosmetic creatureCosmetic)
    {
        var ccgData = ((PlayerGraphics)player.graphicsModule).GetPlayerGraphicsCCGData();
        // Add the cosmetic to the player graphics data.
        ccgData.cosmetics.Add(creatureCosmetic);
        // Add the cosmetic to the correct layer.
        for (int i = 0; i < creatureCosmetic.SpriteLayerGroups.Length; i++)
        {
            Plugin.LogDebug($"Adding cosmetic with sprite {creatureCosmetic.SLeaser.sprites[creatureCosmetic.SpriteLayerGroups[i].firstSpriteIndex].element.name} to layer {creatureCosmetic.SpriteLayerGroups[i].layer}");
            ccgData.layersCosmetics[creatureCosmetic.SpriteLayerGroups[i].layer].Add(creatureCosmetic);
        }
    }

    public static void EquipSlugcatCosmeticsPreset(this Player player, SlugcatCosmeticsPreset preset)
    {
        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetPlayerGraphicsCCGData();

        ccgData.cosmeticsPreset = preset;
        for (int i = 0; i < preset.dynamicCosmetics.Count; i++)
        {
            var presetDynamicCosmeticTypeId = preset.dynamicCosmetics[i].Item1;
            string presetPropertiesId = preset.dynamicCosmetics[i].Item2;

            CosmeticManager.DynamicCosmeticTypeInfo? presetDynamicCosmeticTypeInfo = CosmeticManager.GetCosmeticTypeInfoFromCosmeticTypeID(presetDynamicCosmeticTypeId);
            var presetDynamicCosmeticType = presetDynamicCosmeticTypeInfo.Value.CosmeticType;
            var presetPropertiesType = presetDynamicCosmeticTypeInfo.Value.PropertiesType;

            // TODO: make a function to create a dynamic cosmetic from the type id and properties id inside of each cosmetic's code, overgoing this stupidity.
            var properties = CosmeticManager.GetLoadedDynamicCosmeticProperties<typeof(presetPropertiesType))>(presetDynamicCosmeticTypeId, presetPropertiesId);

            IDynamicCreatureCosmetic creatureCosmetic = (IDynamicCreatureCosmetic)Activator.CreateInstance(presetDynamicCosmeticType, player, properties);
            EquipDynamicSlugcatCosmetic(player, creatureCosmetic);
        }

        ccgData.compartmentalizedGraphicsEnabled = true;
    }
}
