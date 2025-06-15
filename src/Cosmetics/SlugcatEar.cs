using UnityEngine;
using RWCustom;

using MRCustom.Math;

namespace CompartmentalizedCreatureGraphics;

public class SlugcatEar : SlugcatCosmetic
{
    public Color earColor;

    public int defaultScaleX = 1;

    public Vector2 pos;
    public Vector2 lastPos;
    public float rad = 5f;

    public Vector2 defaultPosOffsetFromHead = new Vector2(-5, 5);

    public FSprite EarSprite
    {
        get { return sLeaser.sprites[0]; }
    }

    private SharedPhysics.TerrainCollisionData scratchTerrainCollisionData;

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        this.sLeaser = sLeaser;

        sLeaser.sprites = new FSprite[1];
        sLeaser.sprites[0] = new FSprite("slugcatEar0", true);

        AddToContainer(sLeaser, rCam, null);
    }

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
        if (player == null)
            return;

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsData = playerGraphics.GetPlayerGraphicsCraftingData();

        if (playerGraphicsData.sLeaser == null)
            return;

        //-- MR7: To achieve the effect of being behind we make get an offset from face angle different to position the head.
        var offsetFaceAngleForBehindHeadPosX = playerGraphicsData.OriginalFaceSprite.x - playerGraphicsData.HeadSprite.x;
        var offsetFaceAngleForBehindHeadPosY = playerGraphicsData.OriginalFaceSprite.y - playerGraphicsData.HeadSprite.y;
        //-- Loop through and update all sprites behind the head + in front of face match the face sprites sprite.
        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].rotation = playerGraphicsData.OriginalFaceSprite.rotation;
            sLeaser.sprites[i].color = earColor;
        }

        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(player.bodyChunks[1].pos, player.mainBodyChunk.pos);

        EarSprite.scaleX = defaultScaleX;

        // If we are moving and facing far a direction, make both ears rotate that way
        //-- MR7: Disabled for now, implement later, was being a pain.
        /*
        if (Math.Abs(playerGraphics.lookDirection.x) > 0.7f)
        {
            var signLookDir = MarMath.NonzeroSign(playerGraphics.lookDirection.x);
            signHeadFaceDir = signLookDir;

            EarSprite.scaleX = signLookDir;
        }
        */

        //-- MR7: 1 distance = 1 pixel in RW, and base ears are positioned 5 pixels out.
        var earPosOffset = defaultPosOffsetFromHead;
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
            playerGraphicsData.HeadSprite.x + earPosOffset.x,
            playerGraphicsData.HeadSprite.y + earPosOffset.y
            );

        var earFinalRotation = playerGraphicsData.HeadSprite.rotation + earRotationOffset;

        // Rotate the ears based on their offset around the heads center point.
        earPosOffset = Custom.RotateAroundVector(earPosOffset, Vector2.zero, playerGraphicsData.HeadSprite.rotation + earPosAroundHeadRotationDegreesOffset);

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

        EarSprite.x = pos.x;
        EarSprite.y = pos.y;
        EarSprite.rotation = earFinalRotation;
        EarSprite.color = playerGraphicsData.HeadSprite.color;

        lastPos = pos;
    }

    public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {

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

        var playerGraphicsData = ((PlayerGraphics)player.graphicsModule).GetPlayerGraphicsCraftingData();
        if (playerGraphicsData.sLeaser != null)
        {
            sLeaser.sprites[0].MoveBehindOtherNode(playerGraphicsData.HeadSprite);
        }
    }
}