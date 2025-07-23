namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatFaceCosmetic : DynamicSlugcatCosmetic
{
    /// <summary>
    /// The Default position set for the cosmetic at each angle.
    /// All calculations are done with half the length treated as the center value.
    /// Currently only supports up to two index in either direction.
    /// </summary>
    public Vector2[] defaultAnglePositions = new Vector2[] { Vector2.zero };

    protected Vector2 posOffset = Vector2.zero;

    /// <summary>
    /// Returns sLeaser.sprites[0]
    /// </summary>
    public FSprite Sprite
    {
        get { return sLeaser.sprites[0]; }
        set { sLeaser.sprites[0] = value; }
    }

    public float defaultScaleX = 1f;

    public string spriteName = "marError64";

    public Color spriteColor = Color.green;

    public int side = 0;

    public float snapValue = 0f;

    public DynamicSlugcatFaceCosmetic(SpriteLayerGroup[] spriteLayerGroups) : base(spriteLayerGroups)
    {
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        base.InitiateSprites(sLeaser, rCam);

        sLeaser.sprites = new FSprite[1];
        Sprite = new FSprite(spriteName + "A0", true);
        Sprite.color = spriteColor;

        AddToContainer(sLeaser, rCam, null);
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        var currentAnglePosIndex = Mathf.Clamp(defaultAnglePositions.Length / 2 + playerGraphicsCCGData.faceAngleNum, 0, defaultAnglePositions.Length - 1);
        posOffset = defaultAnglePositions[currentAnglePosIndex];

        float sidedScale = 1f;
        if (side != 0 && side != playerGraphicsCCGData.faceSide)
            sidedScale = -1f;

        var finalRotation = MarMathf.Snap(playerGraphicsCCGData.faceRotation, snapValue);
        
        var rotatedPosOffset = Custom.RotateAroundVector(posOffset, Vector2.zero, playerGraphicsCCGData.faceRotation);
        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].x = playerGraphicsCCGData.facePos.x + rotatedPosOffset.x;
            sLeaser.sprites[i].y = playerGraphicsCCGData.facePos.y + rotatedPosOffset.y;
            // MR7: TODO: change this to be the rotation AROUND the facePos instead.
            // Something something relative value child rotation magic mathy garbage, it's REQUIRED for the slugcat to not look god awful when flipped upside down lol.
            sLeaser.sprites[i].rotation = finalRotation;
            sLeaser.sprites[i].scaleX = defaultScaleX * sidedScale;
        }
    }

    // TODO: temp thing for proof of concept, later use cosmetic effects.
    public override void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {
        if (sLeaser == null)
            return;

        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].color = palette.blackColor;
        }
    }
}
