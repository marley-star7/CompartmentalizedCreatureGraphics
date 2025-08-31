using CompartmentalizedCreatureGraphics.Extensions;

namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmeticEar : DynamicSlugcatFaceCosmetic
{
    public new class Properties : DynamicSlugcatFaceCosmetic.Properties
    {
        //-- MS7: The default curl value, from 0 to 1.
        [JsonProperty("curl")]
        public float curl = 0;

        [JsonProperty("rotationOffsetDegrees")]
        public float rotationOffsetDegrees = 0;

        [JsonProperty("rad")]
        public float rad = 5f;
    }

    public new Properties properties => (Properties)_properties;

    public Vector2 pos;
    public Vector2 lastPos;

    private SharedPhysics.TerrainCollisionData scratchTerrainCollisionData;

    public DynamicSlugcatCosmeticEar(PlayerGraphics wearerGraphics, Properties properties) : base(wearerGraphics, properties)
    {

    }
    
    public override void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (sLeaser == null)
            return;

        lastPos = pos;

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsData = playerGraphics.GetPlayerGraphicsCCGData();

        anglePosOffset = GetAnglePosOffset(playerGraphicsData.faceSpriteAngleNum);
        sidedScale = GetSidedScale(playerGraphicsData.faceSide);

        Vector2 faceRotationTimeStacked = Vector2.Lerp(playerGraphicsData.lastFaceRotation, playerGraphicsData.faceRotation, timeStacker);
        var faceRotationDegreesTimeStacked = Custom.VecToDeg(faceRotationTimeStacked);

        float earRotationOffset = properties.rotationOffsetDegrees;

        // MS7: Makes it so the ear rotates the right way depending on angle and side.
        float earRotationSide = properties.side * -sidedScale;

        // Base rotation offset - multiplied by side to flip direction appropriately
        earRotationOffset += faceRotationTimeStacked.x * -60 * earRotationSide;

        //-- MS7: TODO: make this curl be the baseline and then lerp to the max curve of 90 threat increase, rather than just the base add it is now, so it looks more natural.
        var maxEarRotation = 90f;
        var earCurlAmount = properties.curl;

        //-- MS7: If player is under threat, make their ears shift down a bit more. 
        var threat = player.GetThreat();
        if (threat > 0.01f)
            earCurlAmount += Mathf.Lerp(0.2f, 1, threat);

        earRotationOffset += (earCurlAmount * maxEarRotation) * earRotationSide;

        // Final rotation calculation with proper side consideration
        var earFinalRotation = (faceRotationDegreesTimeStacked * earRotationSide) + earRotationOffset;

        // Rotate the ears based on their offset around the heads center point.
        //earPosOffset = Custom.RotateAroundVector(earPosOffset, Vector2.zero, playerGraphicsData.BaseHeadSprite.rotation + earPosAroundHeadRotationDegreesOffset);

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

        //-- MS7: If player is sideways, offset the rotation around the head based how much.
        // Apply side multiplier to horizontal direction influence
        pos = playerGraphicsData.BaseHeadSprite.GetPosition() + Custom.RotateAroundVector(anglePosOffset, Vector2.zero, faceRotationDegreesTimeStacked * earRotationSide);
        var finalPos = Vector2.Lerp(lastPos, pos, timeStacker);

        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            var currentSprite = sLeaser.sprites[i];

            currentSprite.x = finalPos.x;
            currentSprite.y = finalPos.y;
            currentSprite.rotation = earFinalRotation;
            currentSprite.color = playerGraphicsData.BaseHeadSprite.color;
            currentSprite.scaleX = properties.scaleX * sidedScale;
            currentSprite.scaleY = properties.scaleY;
        }
    }
}