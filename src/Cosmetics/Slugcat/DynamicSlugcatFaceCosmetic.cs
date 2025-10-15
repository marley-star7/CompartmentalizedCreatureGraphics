using CompartmentalizedCreatureGraphics.Extensions;
using Newtonsoft.Json;
using System.IO;

namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatFaceCosmetic : DynamicSlugcatCosmetic
{
    public new class Properties : DynamicSlugcatCosmetic.Properties
    {
        [JsonProperty("spriteNames")]
        public string[] spriteNames = { "marError64" };

        public Color spriteColor = Color.green;

        [JsonProperty("side")]
        public int side = 0;

        [JsonProperty("snap")]
        public float snap = 15f;

        /// <summary>
        /// 1 int in distance = 1 pixel in RW,
        /// Ex: base ears are positioned 5 pixels out.
        /// </summary>
        [JsonProperty("spriteAnglePropertiesId")]
        public string spriteAnglePropertiesId = "";

        public SpriteAngleProperties spriteAngleProperties = new(new Vector2[] { Vector2.zero });

        public override DynamicCreatureCosmetic.Properties Parse(string json)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };

            Properties properties = JsonConvert.DeserializeObject<Properties>(json, settings);
            properties.spriteAngleProperties = CCG.CosmeticManager.GetSpriteAnglePropertiesForId(properties.spriteAnglePropertiesId);

            return properties;
        }
    }

    public new Properties properties => (Properties)_properties;

    protected Vector2 anglePosOffset = Vector2.zero;
    protected float sidedScale = 0;

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
    }

    protected Vector2 GetAnglePosOffset(in int faceSpriteAngleNum)
    {
        var currentAnglePosIndex = Mathf.Clamp(properties.spriteAngleProperties.positions.Length / 2 + faceSpriteAngleNum, 0, properties.spriteAngleProperties.positions.Length - 1);
        return properties.spriteAngleProperties.positions[currentAnglePosIndex];
    }

    protected float GetSidedScale(in int faceSide)
    {
        sidedScale = 1f;
        if (properties.side != 0 && properties.side != faceSide)
            sidedScale = -1f;

        return sidedScale;
    }

    public override void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (SLeaser == null)
            return;

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        anglePosOffset = GetAnglePosOffset(playerGraphicsCCGData.faceSpriteAngleNum);
        sidedScale = GetSidedScale(playerGraphicsCCGData.faceSide);

        var faceRotationTimeStacked = Vector2.Lerp(playerGraphicsCCGData.lastFaceRotation, playerGraphicsCCGData.faceRotation, timeStacker);
        var finalRotation = Custom.VecToDeg(faceRotationTimeStacked);
        finalRotation = MarMathf.Snap(Custom.VecToDeg(faceRotationTimeStacked), properties.snap);

        var rotatedPosOffset = Custom.RotateAroundVector(anglePosOffset, Vector2.zero, finalRotation);

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
        if (SLeaser == null)
            return;

        for (int i = 0; i < _sLeaser.sprites.Length; i++)
        {
            _sLeaser.sprites[i].color = palette.blackColor;
        }
    }
}
