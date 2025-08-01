using CompartmentalizedCreatureGraphics.Extensions;
using System.Xml.Linq;

namespace CompartmentalizedCreatureGraphics;

internal static class PlayerGraphicsHooks
{
    internal static void PlayerGraphics_ctor(On.PlayerGraphics.orig_ctor orig, PlayerGraphics self, PhysicalObject ow)
    {
        orig(self, ow);

        var selfCCGData = self.GetPlayerGraphicsCCGData();
    }

    /// <summary>
    /// MR7: We do all of our calculations of saving details about chunk positioning and stuff in here, since it only occurs every update (40fps) anyways.
    /// Then we do the time stacked variables stuff in drawSprites since that runs every frame.
    /// </summary>
    /// <param name="orig"></param>
    /// <param name="self"></param>
    internal static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);
        var playerGraphicsCCGData = self.GetPlayerGraphicsCCGData();

        playerGraphicsCCGData.lastFaceRotation = playerGraphicsCCGData.faceRotation;

        Vector2 lastDirLowerChunkToMainChunk = Custom.DirVec(self.player.bodyChunks[1].pos, self.player.mainBodyChunk.pos);
        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(self.player.bodyChunks[1].pos, self.player.mainBodyChunk.pos);

        playerGraphicsCCGData.faceRotation = Custom.VecToDeg(dirLowerChunkToMainChunk);
        playerGraphicsCCGData.faceRotation -= dirLowerChunkToMainChunk.x * 90;
    }

    //
    // IDRAWABLE
    //

    internal static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        var playerGraphicsCCGData = self.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;

        var player = self.player;

        orig(self, sLeaser, rCam);

        player.AddOriginalPlayerGraphicsCosmeticReference();

        if (PresetManager.defaultSlugcatCosmeticsPresets.ContainsKey(self.player.slugcatStats.name))
            player.EquipSlugcatCosmeticsPreset(PresetManager.GetDefaultSlugcatCosmeticsPreset(self.player.slugcatStats.name));

        //-- MR7: Currently we just re-order again even though this runs twice techincally, since base thing adds to container, but this works for now in solving the layering issue.
        self.AddDynamicCosmeticsToContainer(sLeaser, rCam, null);
    }

    internal static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var selfCCGData = self.GetPlayerGraphicsCCGData();
        selfCCGData.sLeaser = sLeaser;

        orig(self, sLeaser, rCam, timeStacker, camPos);

        //-- MR7: For compatability, only do slugcat compartmentalized graphics if it is enabled for this scug.
        // By default, it is not.
        if (!selfCCGData.compartmentalizedGraphicsEnabled)
            return;

        Vector2 lastDirLowerChunkToMainChunk = Custom.DirVec(self.player.bodyChunks[1].lastPos, self.player.mainBodyChunk.lastPos);
        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(self.player.bodyChunks[1].pos, self.player.mainBodyChunk.pos);
        Vector2 dirLowerChunkToMainChunkTimeStacked = Vector2.Lerp(lastDirLowerChunkToMainChunk, dirLowerChunkToMainChunk, timeStacker);

        //
        //-- Base Sprites Replacings (We do this instead of just "removing" sprites[9] for compatability purposes)
        //

        if (selfCCGData.BaseHeadSprite == null)
            Plugin.LogError("BaseHeadSprite is null, this should not happen!");
        else
        {
            var headSpriteNameAngled = selfCCGData.cosmeticsPreset.baseHeadSpriteName + "A0";
            selfCCGData.BaseHeadSprite.element = Futile.atlasManager.GetElementWithName(headSpriteNameAngled);
        }

        var player = self.player;

        if (selfCCGData.BaseFaceSprite == null)
            Plugin.LogError("BaseFaceSprite is null, this should not happen!");
        else
        {
            // Temp testing
            selfCCGData.BaseFaceSprite.element = Futile.atlasManager.GetElementWithName("marNothing");
            selfCCGData.BaseFaceSprite.color = Color.green;

            selfCCGData.facePos = new Vector2(selfCCGData.BaseFaceSprite.x, selfCCGData.BaseFaceSprite.y);
        }

        //-- MR7: If player is sideways and not in zero g, offset the face sprite rotation around the head relative to how horizontal.
        // This is to fake the effect of how the current head turns sideways on horizontals such as when crouching or flipping.
        // (It also makes flips look alot more weighty and rad)

        if (player.room != null && player.EffectiveRoomGravity == 0f)
        {
            self.SetFaceAngle(0);
        }
        //-- MR7: Taken from source to calculate when using side angles.
        else if ((player.bodyMode == Player.BodyModeIndex.Stand && player.input[0].x != 0) || player.bodyMode == Player.BodyModeIndex.Crawl)
        {
            var correctAngle = MarMath.NonzeroSign(dirLowerChunkToMainChunkTimeStacked.x) * 2;
            self.SetFaceAngle(correctAngle);
        }
        else
        {
            // The further the body is rotated sideways, swap the face angles to sideways.
            switch (dirLowerChunkToMainChunkTimeStacked.x)
            {
                case > 0.9f:
                    self.SetFaceAngle(2);
                    break;

                case > 0.45f:
                    self.SetFaceAngle(2);
                    break;

                case < -0.9f:
                    self.SetFaceAngle(-2);
                    break;

                case < -0.45f:
                    self.SetFaceAngle(-2);
                    break;

                default:
                    self.SetFaceAngle(0);
                    break;

            }
        }

        selfCCGData.faceRotationTimeStacked = Mathf.Lerp(selfCCGData.lastFaceRotation, selfCCGData.faceRotation, timeStacker);

        // Finally draw all the cosmetics.
        for (int i = 0; i < selfCCGData.cosmetics.Count; i++)
        {
            selfCCGData.cosmetics[i].OnWearerDrawSprites(sLeaser, rCam, timeStacker, camPos);
        }
    }

    internal static void PlayerGraphics_ApplyPalette(On.PlayerGraphics.orig_ApplyPalette orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam, palette);

        for (int i = 0; i < playerGraphicsCCGData.cosmetics.Count; i++)
        {
            playerGraphicsCCGData.cosmetics[i].OnWearerApplyPalette(sLeaser, rCam, in palette);
        }
    }

    internal static void PlayerGraphics_AddToContainer(On.PlayerGraphics.orig_AddToContainer orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
    {
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam, newContatiner);

        playerGraphics.AddDynamicCosmeticsToContainer(sLeaser, rCam, newContatiner);
    }
}
