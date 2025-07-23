namespace CompartmentalizedCreatureGraphics;

internal static class PlayerGraphicsHooks
{
    internal static void PlayerGraphics_ctor(On.PlayerGraphics.orig_ctor orig, PlayerGraphics self, PhysicalObject ow)
    {
        orig(self, ow);

        var selfCCGData = self.GetPlayerGraphicsCCGData();
    }

    internal static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);
    }

    //
    // IDRAWABLE
    //

    internal static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        var playerGraphicsCCGData = self.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(self, sLeaser, rCam);

        var player = self.player;

        self.EquipCosmetic(self.CreateBasePlayerGraphicsReferenceCosmetic());

        if (Content.characterCosmeticPresets.ContainsKey(self.player.slugcatStats.name))
            self.EquipSlugcatCosmeticsPreset(Content.characterCosmeticPresets[self.player.slugcatStats.name]);
    }

    internal static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam, timeStacker, camPos);

        //-- MR7: For compatability, only do slugcat compartmentalized graphics if it is enabled for this scug.
        // By default, it is not.
        if (!playerGraphicsCCGData.compartmentalizedGraphicsEnabled)
            return;

        //-- Replace the original sprites.
        // We do this instead of just "removing" sprites[9] for compatability purposes...

        if (playerGraphicsCCGData.BaseHeadSprite == null)
            Plugin.DebugError("BaseHeadSprite is null, this should not happen!");
        else
            playerGraphicsCCGData.BaseHeadSprite.element = Futile.atlasManager.GetElementWithName(playerGraphicsCCGData.cosmeticsPreset.baseHeadSpriteName);

        var player = playerGraphics.player;

        if (playerGraphicsCCGData.BaseFaceSprite == null)
            Plugin.DebugError("BaseFaceSprite is null, this should not happen!");
        else
        {
            // Temp testing
            playerGraphicsCCGData.BaseFaceSprite.element = Futile.atlasManager.GetElementWithName("marNothing");
            playerGraphicsCCGData.BaseFaceSprite.color = Color.green;

            playerGraphicsCCGData.facePos = new Vector2(playerGraphicsCCGData.BaseFaceSprite.x, playerGraphicsCCGData.BaseFaceSprite.y);
        }

        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(playerGraphics.player.bodyChunks[1].pos, playerGraphics.player.mainBodyChunk.pos);

        //-- MR7: If player is sideways and not in zero g, offset the face sprite rotation around the head relative to how horizontal.
        // This is to fake the effect of how the current head turns sideways on horizontals such as when crouching or flipping.
        // (It also makes flips look alot more weighty and rad)
        if (player.room != null && player.EffectiveRoomGravity == 0f)
        {
            playerGraphics.SetFaceAngle(0);
        }
        //-- MR7: Taken from source to calculate when using side angles.
        else if ((player.bodyMode == Player.BodyModeIndex.Stand && player.input[0].x != 0) || player.bodyMode == Player.BodyModeIndex.Crawl)
        {
            var correctAngle = MarMath.NonzeroSign(dirLowerChunkToMainChunk.x) * 2;
            playerGraphics.SetFaceAngle(correctAngle);
        }
        else
        {
            // The further the body is rotated sideways, swap the face angles to sideways.
            switch (dirLowerChunkToMainChunk.x)
            {
                case > 0.9f:
                    playerGraphics.SetFaceAngle(2);
                    break;

                case > 0.45f:
                    playerGraphics.SetFaceAngle(2);
                    break;

                case < -0.9f:
                    playerGraphics.SetFaceAngle(-2);
                    break;

                case < -0.45f:
                    playerGraphics.SetFaceAngle(-2);
                    break;

                default:
                    playerGraphics.SetFaceAngle(0);
                    break;

            }

            playerGraphicsCCGData.faceRotation = Custom.VecToDeg(dirLowerChunkToMainChunk);
            playerGraphicsCCGData.faceRotation -= dirLowerChunkToMainChunk.x * 90;

            // Disabled cuz looks bad lol.
            /*
            float faceAngleAllowance = (dirLowerChunkToMainChunk.x + playerGraphics.lookDirection.x);
            // Normal lookage, small change for cooool effect of slight head turn at high angles.
            switch (faceAngleAllowance)
            {
                case > 0.9f:
                    playerGraphics.SetFaceAngle(1);
                    //playerGraphicsCCGData.facePos += new Vector2(2, -1);
                    break;

                case < -0.9f:
                    playerGraphics.SetFaceAngle(-1);
                    //playerGraphicsCCGData.facePos += new Vector2(-2, -1);
                    break;

                default:
                    playerGraphics.SetFaceAngle(0);
                    break;
            }
            */
        }

        // Finally draw all the cosmetics.
        for (int i = 0; i < playerGraphicsCCGData.cosmetics.Count; i++)
        {
            playerGraphicsCCGData.cosmetics[i].OnWearerDrawSprites(sLeaser, rCam, timeStacker, camPos);
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
