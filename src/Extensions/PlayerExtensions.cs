using CompartmentalizedCreatureGraphics.Cosmetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Extensions;

public static class PlayerExtensions
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

    public static void EquipDynamicSlugcatCosmetic(this Player player, string cosmeticTypeId, string propertiesId)
    {
        Plugin.LogDebug($"Equipping cosmeticTypeID {cosmeticTypeId} of propertiesID {propertiesId} to player");
        CosmeticManager.GetCritcosFromCosmeticTypeId(cosmeticTypeId).CreateDynamicCosmeticForPlayer(player, propertiesId);
    }

    public static void EquipSlugcatCosmeticsPreset(this Player player, SlugcatCosmeticsPreset preset)
    {
        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var ccgData = playerGraphics.GetPlayerGraphicsCCGData();

        ccgData.cosmeticsPreset = preset;
        for (int i = 0; i < preset.dynamicCosmetics.Count; i++)
        {
            EquipDynamicSlugcatCosmetic(player, preset.dynamicCosmetics[i].cosmeticTypeId, preset.dynamicCosmetics[i].propertiesId);
        }

        ccgData.compartmentalizedGraphicsEnabled = true;
    }
}
