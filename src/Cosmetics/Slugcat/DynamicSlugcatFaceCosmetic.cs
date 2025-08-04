using Newtonsoft.Json;
using System.IO;

namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatFaceCosmetic : DynamicSlugcatCosmetic
{
    public new class Properties : DynamicCreatureCosmetic.Properties
    {
        /// <summary>
        /// The Default position set for the cosmetic at each angle.
        /// All calculations are done with half the length treated as the center value.
        /// Currently only supports up to two index in either direction.
        /// </summary>
        [JsonProperty("anglePositions")]
        [JsonConverter(typeof(Vector2ArrayJsonConverter))]
        public Vector2[] anglePositions = new Vector2[]{Vector2.zero};

        [JsonProperty("spriteNames")]
        public string[] spriteNames = { "marError64" };

        public Color spriteColor = Color.green;

        [JsonProperty("side")]
        public int side = 0;

        [JsonProperty("snap")]
        public float snap = 0f;
    }

    public new Properties properties => (Properties)_properties;

    protected Vector2 posOffset = Vector2.zero;

    public DynamicSlugcatFaceCosmetic(PlayerGraphics wearerGraphics, Properties properties) : base(wearerGraphics, properties)
    {
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        base.InitiateSprites(sLeaser, rCam);

        sLeaser.sprites = new FSprite[properties.spriteNames.Length];

        for (int i = 0; i < properties.spriteNames.Length; i++)
        {
            sLeaser.sprites[i] = new FSprite(properties.spriteNames[0] + "A0", true);
            sLeaser.sprites[i].color = properties.spriteColor;
        }

        AddToContainer(sLeaser, rCam, null);
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (sLeaser == null)
            return;

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        var currentAnglePosIndex = Mathf.Clamp(properties.anglePositions.Length / 2 + playerGraphicsCCGData.faceAngleNum, 0, properties.anglePositions.Length - 1);
        posOffset = properties.anglePositions[currentAnglePosIndex];

        float sidedScale = 1f;
        if (properties.side != 0 && properties.side != playerGraphicsCCGData.faceSide)
            sidedScale = -1f;

        var finalRotation = MarMathf.Snap(playerGraphicsCCGData.faceRotationTimeStacked, properties.snap);

        var rotatedPosOffset = Custom.RotateAroundVector(posOffset, Vector2.zero, finalRotation);
        for (int i = 0; i < _sLeaser.sprites.Length; i++)
        {
            _sLeaser.sprites[i].x = playerGraphicsCCGData.facePos.x + rotatedPosOffset.x;
            _sLeaser.sprites[i].y = playerGraphicsCCGData.facePos.y + rotatedPosOffset.y;
            _sLeaser.sprites[i].rotation = finalRotation;
            _sLeaser.sprites[i].scaleX = properties.scaleX * sidedScale;
            _sLeaser.sprites[i].scaleY = properties.scaleY;
        }
    }

    // TODO: temp thing for proof of concept, later use cosmetic effects.
    public override void ApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, RoomPalette palette)
    {
        if (sLeaser == null)
            return;

        for (int i = 0; i < _sLeaser.sprites.Length; i++)
        {
            _sLeaser.sprites[i].color = palette.blackColor;
        }
    }
}
