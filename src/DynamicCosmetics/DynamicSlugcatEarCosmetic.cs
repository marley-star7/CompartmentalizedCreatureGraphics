using UnityEngine;
using RWCustom;

using MRCustom.Math;
using CompartmentalizedCreatureGraphics.Extensions;

namespace CompartmentalizedCreatureGraphics.SlugcatCosmetics;

public class DynamicSlugcatEar : DynamicSlugcatFaceCosmetic
{
    public Color earColor;

    public Vector2 pos;
    public Vector2 lastPos;
    public float rad = 5f;

    private SharedPhysics.TerrainCollisionData scratchTerrainCollisionData;

    private float GetPlayerThreat()
    {
        if (player == null)
            return 0f;
        if (player.abstractCreature.world.game.GameOverModeActive)
            return 0f;
        if (player.abstractCreature.world.game.manager.musicPlayer != null && player.abstractCreature.world.game.manager.musicPlayer.threatTracker != null)
            return player.abstractCreature.world.game.manager.musicPlayer.threatTracker.currentMusicAgnosticThreat;
        if (player.abstractCreature.world.game.manager.fallbackThreatDetermination == null)
            player.abstractCreature.world.game.manager.fallbackThreatDetermination = new ThreatDetermination(0);

        return player.abstractCreature.world.game.manager.fallbackThreatDetermination.currentMusicAgnosticThreat;
    }
    
    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        base.OnWearerDrawSprites(wearerSLeaser, rCam, timeStacker, camPos);

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsData = playerGraphics.GetPlayerGraphicsCCGData();

        //-- MR7: To achieve the effect of being behind we make get an offset from face angle different to position the head.
        var offsetFaceAngleForBehindHeadPosX = playerGraphicsData.OriginalFaceSprite.x - playerGraphicsData.OriginalHeadSprite.x;
        var offsetFaceAngleForBehindHeadPosY = playerGraphicsData.OriginalFaceSprite.y - playerGraphicsData.OriginalHeadSprite.y;
        //-- Loop through and update all sprites behind the head + in front of face match the face sprites sprite.
        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].color = earColor;
        }

        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(player.bodyChunks[1].pos, player.mainBodyChunk.pos);

        //-- MR7: 1 distance = 1 pixel in RW, and base ears are positioned 5 pixels out.
        var earPosOffset = posOffset;
        float earPosAroundHeadRotationDegreesOffset = 0f;

        float earRotationOffset = 0f;

        //-- MR7: If player is sideways, offset the rotation around the head based how much.
        earPosAroundHeadRotationDegreesOffset -= dirLowerChunkToMainChunk.x * 90;

        earRotationOffset += dirLowerChunkToMainChunk.x * -60; // Mulitply by scale so it rotates similarly to base game scug ears.

        //-- MR7: If player is under threat, make their ears shift down a bit more. 

        var threat = GetPlayerThreat();
        if (threat > 0.01f)
        {
            var maxEarRotation = 90f;
            var earRotationFromThreatModifier = Mathf.Lerp(0.2f, 1, threat);

            earRotationOffset += earRotationFromThreatModifier * maxEarRotation;
        }

        pos = new Vector2(
            playerGraphicsData.OriginalHeadSprite.x + earPosOffset.x,
            playerGraphicsData.OriginalHeadSprite.y + earPosOffset.y
            );

        var earFinalRotation = Custom.VecToDeg(dirLowerChunkToMainChunk) + earRotationOffset;

        // Rotate the ears based on their offset around the heads center point.
        //earPosOffset = Custom.RotateAroundVector(earPosOffset, Vector2.zero, playerGraphicsData.OriginalHeadSprite.rotation + earPosAroundHeadRotationDegreesOffset);

        // TODO: get this working.
        /*
        // The physicsy stuff, to make the ears bend with the terrain and walls.
        SharedPhysics.TerrainCollisionData collisionData = scratchTerrainCollisionData.Set(pos, lastPos, new Vector2(0, -2f), rad, new IntVector2(0, 0), true);
        collisionData = SharedPhysics.VerticalCollision(player.room, collisionData);

        if (collisionData.contactPoint != new IntVector2(0, 0))
        {
            // Lerp the ear rotation to fully down if a contact point was found.
            earFinalRotation = Mathf.Lerp(earFinalRotation, 90, 1);
        }
        */

        Sprite.x = pos.x;
        Sprite.y = pos.y;
        Sprite.rotation = earFinalRotation;
        Sprite.color = playerGraphicsData.OriginalHeadSprite.color;

        lastPos = pos;
    }

    public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        newContainer ??= rCam.ReturnFContainer("Midground");

        foreach (FSprite fsprite in sLeaser.sprites)
        {
            fsprite.RemoveFromContainer();
            newContainer.AddChild(fsprite);
        }

        if (player == null)
            return;

        var playerGraphicsData = ((PlayerGraphics)player.graphicsModule).GetPlayerGraphicsCCGData();
        if (playerGraphicsData.sLeaser != null)
        {
            sLeaser.sprites[0].MoveBehindOtherNode(playerGraphicsData.OriginalHeadSprite);
        }
    }
}