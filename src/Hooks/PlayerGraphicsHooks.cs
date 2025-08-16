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

        //-- MS7: If player is sideways and not in zero g, offset the face sprite rotation around the head relative to how horizontal.
        // This is to fake the effect of how the current head turns sideways on horizontals such as when crouching or flipping.
        // (It also makes flips look alot more weighty and rad)

        if (player.room != null && player.EffectiveRoomGravity == 0f)
        {
            self.SetFaceAngle(0);
        }
        //-- MS7: Taken from source to calculate when using side angles.
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
                    self.SetFaceAngle(1);
                    break;

                case < -0.9f:
                    self.SetFaceAngle(-2);
                    break;

                case < -0.45f:
                    self.SetFaceAngle(-1);
                    break;

                default:
                    self.SetFaceAngle(0);
                    break;

            }
        }

        selfCCGData.faceRotationTimeStacked = Mathf.Lerp(selfCCGData.lastFaceRotation, selfCCGData.faceRotation, timeStacker);
    }
}
