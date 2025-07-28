using System.IO;

namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatFaceCosmetic : DynamicSlugcatCosmetic
{
    public class Properties : DynamicCreatureCosmetic.Properties
    {
        /// <summary>
        /// The Default position set for the cosmetic at each angle.
        /// All calculations are done with half the length treated as the center value.
        /// Currently only supports up to two index in either direction.
        /// </summary>
        public Vector2[] anglePositions = new Vector2[] { Vector2.zero };

        public string spriteName = "marError64";

        public Color spriteColor = Color.green;

        public int side = 0;

        public float snapValue = 0f;

        public Properties()
        {
            spriteName = "marError64";
            spriteColor = Color.green;
            side = 0;
            snapValue = 0f;
        }
    }
    public Properties properties;

    protected Vector2 posOffset = Vector2.zero;

    /// <summary>
    /// Returns sLeaser.sprites[0]
    /// </summary>
    public FSprite Sprite
    {
        get { return _sLeaser.sprites[0]; }
        set { _sLeaser.sprites[0] = value; }
    }

    public DynamicSlugcatFaceCosmetic(Player wearer, Properties properties) : base(wearer, properties.spriteLayerGroups)
    {
        /*
        var path = AssetManager.ListDirectory("ccg");
        var deserializedProperties = Json.Parser.Parse(File.ReadAllText(path)) as Dictionary<string, object>;
        */
        this.properties = properties;
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        base.InitiateSprites(sLeaser, rCam);

        sLeaser.sprites = new FSprite[1];
        Sprite = new FSprite(properties.spriteName + "A0", true);
        Sprite.color = properties.spriteColor;

        AddToContainer(sLeaser, rCam, null);
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var playerGraphics = (PlayerGraphics)Player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        var currentAnglePosIndex = Mathf.Clamp(properties.anglePositions.Length / 2 + playerGraphicsCCGData.faceAngleNum, 0, properties.anglePositions.Length - 1);
        posOffset = properties.anglePositions[currentAnglePosIndex];

        float sidedScale = 1f;
        if (properties.side != 0 && properties.side != playerGraphicsCCGData.faceSide)
            sidedScale = -1f;

        var finalRotation = MarMathf.Snap(playerGraphicsCCGData.faceRotation, properties.snapValue);
        
        var rotatedPosOffset = Custom.RotateAroundVector(posOffset, Vector2.zero, playerGraphicsCCGData.faceRotation);
        for (int i = 0; i < _sLeaser.sprites.Length; i++)
        {
            _sLeaser.sprites[i].x = playerGraphicsCCGData.facePos.x + rotatedPosOffset.x;
            _sLeaser.sprites[i].y = playerGraphicsCCGData.facePos.y + rotatedPosOffset.y;
            // MR7: TODO: change this to be the rotation AROUND the facePos instead.
            // Something something relative value child rotation magic mathy garbage, it's REQUIRED for the slugcat to not look god awful when flipped upside down lol.
            _sLeaser.sprites[i].rotation = finalRotation;
            _sLeaser.sprites[i].scaleX = properties.scaleX * sidedScale;
            _sLeaser.sprites[i].scaleX = properties.scaleY;
        }
    }

    // TODO: temp thing for proof of concept, later use cosmetic effects.
    public override void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {
        if (_sLeaser == null)
            return;

        for (int i = 0; i < _sLeaser.sprites.Length; i++)
        {
            _sLeaser.sprites[i].color = palette.blackColor;
        }
    }
}
