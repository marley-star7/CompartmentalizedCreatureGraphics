using CompartmentalizedCreatureGraphics.Extensions;
using System.Xml.Linq;

namespace CompartmentalizedCreatureGraphics;

internal static class PlayerGraphicsHooks
{
    internal static void PlayerGraphics_ctor(On.PlayerGraphics.orig_ctor orig, PlayerGraphics self, PhysicalObject ow)
    {
        orig(self, ow);

        self.CreateAndAddOriginalPlayerGraphicsCosmeticReference();

        if (PresetManager.defaultSlugcatCosmeticsPresets.ContainsKey(self.player.slugcatStats.name))
            self.CreateAndAddSlugcatCosmeticsPreset(PresetManager.GetDefaultSlugcatCosmeticsPreset(self.player.slugcatStats.name));
    }

    /// <summary>
    /// MS7: We do all of our calculations of saving details about chunk positioning and stuff in here, since it only occurs every update (40fps) anyways.
    /// Then we do the time stacked variables stuff in drawSprites since that runs every frame.
    /// </summary>
    /// <param name="orig"></param>
    /// <param name="self"></param>
    internal static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);
        var player = self.player;
        var playerGraphicsCCGData = self.GetPlayerGraphicsCCGData();

        playerGraphicsCCGData.lastFaceRotation = playerGraphicsCCGData.faceRotation;
        playerGraphicsCCGData.lastFaceAngleValue = playerGraphicsCCGData.faceAngleValue;

        Vector2 lastDirLowerChunkToMainChunk = Custom.DirVec(self.player.bodyChunks[1].pos, self.player.mainBodyChunk.pos);
        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(self.player.bodyChunks[1].pos, self.player.mainBodyChunk.pos);

        var faceRotationDegrees = Custom.VecToDeg(dirLowerChunkToMainChunk);
        faceRotationDegrees -= dirLowerChunkToMainChunk.x * 90;
        playerGraphicsCCGData.faceRotation = Custom.DegToVec(faceRotationDegrees);

        //-- MS7: If player is sideways and not in zero g, offset the face sprite rotation around the head relative to how horizontal.
        // This is to fake the effect of how the current head turns sideways on horizontals such as when crouching or flipping.
        // (It also makes flips look alot more weighty and rad)

        if (player.room != null && player.EffectiveRoomGravity == 0f
            || player.bodyMode == Player.BodyModeIndex.Stand && player.input[0].x == 0)
        {
            playerGraphicsCCGData.targetFaceAngleValue = 0f;
            playerGraphicsCCGData.faceSpriteAnglingMode = PlayerGraphicsCCGData.FaceSpriteAnglingMode.LerpFaceAngleValue;
        }
        else if (player.Stunned
            || player.bodyMode == Player.BodyModeIndex.Default)
        {
            playerGraphicsCCGData.faceSpriteAnglingMode = PlayerGraphicsCCGData.FaceSpriteAnglingMode.AngleWithBodyDirection;
        }
        else if (player.bodyMode == Player.BodyModeIndex.Stand && player.input[0].x != 0)
        {
            //-- MS7: Taken from source to calculate when using side angles.
            playerGraphicsCCGData.targetFaceAngleValue = player.input[0].x;
            playerGraphicsCCGData.faceSpriteAnglingMode = PlayerGraphicsCCGData.FaceSpriteAnglingMode.LerpFaceAngleValue;
        }
        else if (player.bodyMode == Player.BodyModeIndex.Crawl)
        {
            playerGraphicsCCGData.targetFaceAngleValue = Math.Sign(dirLowerChunkToMainChunk.x);
            playerGraphicsCCGData.faceSpriteAnglingMode = PlayerGraphicsCCGData.FaceSpriteAnglingMode.FullAngleToBodyDirection;
        }
        else
        {
            playerGraphicsCCGData.faceSpriteAnglingMode = PlayerGraphicsCCGData.FaceSpriteAnglingMode.AngleWithBodyDirection;
        }

        playerGraphicsCCGData.faceAngleValue = Mathf.Lerp(playerGraphicsCCGData.faceAngleValue, playerGraphicsCCGData.targetFaceAngleValue, 0.45f);
    }

    //
    // IDRAWABLE
    //

    internal static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var selfCCGData = self.GetPlayerGraphicsCCGData();
        orig(self, sLeaser, rCam, timeStacker, camPos);

        //-- MS7: For compatability, only do slugcat compartmentalized graphics if it is enabled for this scug.
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

        if (selfCCGData.faceSpriteAnglingMode == PlayerGraphicsCCGData.FaceSpriteAnglingMode.LerpFaceAngleValue)
        {
            var faceAngleValueTimeStacked = Mathf.Lerp(selfCCGData.lastFaceAngleValue, selfCCGData.faceAngleValue, timeStacker);
            var faceSpriteAngle = (int)(faceAngleValueTimeStacked * 2.5f); // Goes from -2 to 2, so multiply by 2 from the -1 to 1 it was before.
            self.SetFaceSpriteAngle(faceSpriteAngle);
        }
        else if (selfCCGData.faceSpriteAnglingMode == PlayerGraphicsCCGData.FaceSpriteAnglingMode.FullAngleToBodyDirection)
        {
            //-- MS7: Taken from source to calculate when using side angles.
            var correctAngle = Math.Sign(dirLowerChunkToMainChunkTimeStacked.x) * 2;
            self.SetFaceSpriteAngle(correctAngle);
        }
        else if (selfCCGData.faceSpriteAnglingMode == PlayerGraphicsCCGData.FaceSpriteAnglingMode.AngleWithBodyDirection)
        {
            self.SetFaceSpriteAngle(PlayerGraphicsCCGExtensions.GetFaceAngleForRotation(dirLowerChunkToMainChunkTimeStacked));
        }
    }
}
