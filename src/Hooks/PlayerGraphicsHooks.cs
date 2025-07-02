using CompartmentalizedCreatureGraphics.Extensions;

namespace CompartmentalizedCreatureGraphics.Hooks;

public static class PlayerGraphicsHooks
{
    //-- Add hooks
    internal static void ApplyHooks()
    {
        On.PlayerGraphics.Update += PlayerGraphics_Update;

        On.PlayerGraphics.InitiateSprites += PlayerGraphics_InitiateSprites;
        On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        On.PlayerGraphics.ApplyPalette += PlayerGraphics_ApplyPalette;
    }

    //-- Remove hooks
    internal static void RemoveHooks()
    {
        On.PlayerGraphics.Update -= PlayerGraphics_Update;

        On.PlayerGraphics.InitiateSprites -= PlayerGraphics_InitiateSprites;
        On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        On.PlayerGraphics.ApplyPalette -= PlayerGraphics_ApplyPalette;
    }

    private static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);
    }

    //
    // IDRAWABLE
    //

    // The Golden Sheet of Sprite Bull-Sheet.
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

    private static void PlayerGraphics_InitiateSprites(On.PlayerGraphics.orig_InitiateSprites orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam);

        var player = playerGraphics.player;

        if (player.slugcatStats.name == SlugcatStats.Name.White)
        {
            playerGraphicsCCGData.compartmentalizedGraphicsEnabled = true;
            playerGraphicsCCGData.onInitiateSpritesDynamicCosmeticsToAdd = PlayerGraphicsCCGData.AddDefaultVanillaSurvivorDynamicCosmetics;
        }
        else if (player.slugcatStats.name == SlugcatStats.Name.Yellow)
        {
            playerGraphicsCCGData.compartmentalizedGraphicsEnabled = true;
            playerGraphicsCCGData.onInitiateSpritesDynamicCosmeticsToAdd = PlayerGraphicsCCGData.AddDefaultVanillaSurvivorDynamicCosmetics;
        }
        else if (player.slugcatStats.name == SlugcatStats.Name.Red)
        {
            playerGraphicsCCGData.compartmentalizedGraphicsEnabled = true;
            playerGraphicsCCGData.onInitiateSpritesDynamicCosmeticsToAdd = PlayerGraphicsCCGData.AddDefaultVanillaSurvivorDynamicCosmetics;
        }
        else if (player.slugcatStats.name == SlugcatStats.Name.Night)
        {
            playerGraphicsCCGData.compartmentalizedGraphicsEnabled = true;
            playerGraphicsCCGData.onInitiateSpritesDynamicCosmeticsToAdd = PlayerGraphicsCCGData.AddDefaultVanillaSurvivorDynamicCosmetics;
        }
        else if (ModManager.MSC)
        {
            if (player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Saint)
            {

            }
        }

        if (playerGraphicsCCGData.compartmentalizedGraphicsEnabled)
        {
            playerGraphicsCCGData.onInitiateSpritesDynamicCosmeticsToAdd(playerGraphics);
        }
    }

    private static void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam, timeStacker, camPos);

        //-- MR7: For compatability, only do slugcat compartmentalized graphics if it is enabled for this scug.
        // By default, it is not.
        if (!playerGraphicsCCGData.compartmentalizedGraphicsEnabled)
            return;

        //-- Replace / Hide the original sprites.

        // Replace original scug head with earless one.
        playerGraphicsCCGData.OriginalHeadSprite.element = Futile.atlasManager.GetElementWithName("ccgSlugcatHeadA0");

        var player = playerGraphics.player;

        // Hide the original face since we are using new one.
        // We do this instead of just "removing" sprites[9] for compatability purposes...
        var origFaceSprite = sLeaser.sprites[9];
        //origFaceSprite.element = Futile.atlasManager.GetElementWithName("marNothing");
        origFaceSprite.color = Color.green;

        playerGraphicsCCGData.facePos = new Vector2(origFaceSprite.x, origFaceSprite.y);
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
        for (int i = 0; i < playerGraphicsCCGData.dynamicCosmetics.Count; i++)
        {
            playerGraphicsCCGData.dynamicCosmetics[i].OnWearerDrawSprites(sLeaser, rCam, timeStacker, camPos);
        }
    }

    private static void PlayerGraphics_ApplyPalette(On.PlayerGraphics.orig_ApplyPalette orig, PlayerGraphics playerGraphics, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        playerGraphicsCCGData.sLeaser = sLeaser;
        orig(playerGraphics, sLeaser, rCam, palette);

        for (int i = 0; i < playerGraphicsCCGData.dynamicCosmetics.Count; i++)
        {
            playerGraphicsCCGData.dynamicCosmetics[i].OnWearerApplyPalette(sLeaser, rCam, in palette);
        }
    }
}
