namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatFullHeadAttachedCosmetic : DynamicSlugcatCosmetic
{
    public struct SpriteInfo
    {
        public string name;
        public float distanceFromHeadModifier = 1f;

        public SpriteInfo(string name)
        {
            this.name = name;
        }
    }

    public SpriteInfo[] spritesInfo;

    public DynamicSlugcatFullHeadAttachedCosmetic(SpriteInfo[] spritesInfo, SpriteLayerGroup[] spriteLayerGroups) : base(spriteLayerGroups)
    {
        this.spritesInfo = spritesInfo;
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        this.sLeaser = sLeaser;

        sLeaser.sprites = new FSprite[spritesInfo.Length];

        for (int i = 0; i < spritesInfo.Length; i++)
        {
            sLeaser.sprites[i] = new FSprite(spritesInfo[i].name + "A0", true);
            sLeaser.sprites[i].color = Color.white; // Default color, can be changed later.
        }

        AddToContainer(sLeaser, rCam, null);
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (player == null)
            return;

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        if (playerGraphicsCCGData.sLeaser == null)
            return;

        //-- MR7: To achieve the effect of being behind we make get an offset from face angle different to position the head.
        var offsetFaceAngleForBehindHeadPosX = playerGraphicsCCGData.BaseFaceSprite.x - playerGraphicsCCGData.BaseHeadSprite.x;
        var offsetFaceAngleForBehindHeadPosY = playerGraphicsCCGData.BaseFaceSprite.y - playerGraphicsCCGData.BaseHeadSprite.y;

        //-- Loop through and update all sprites behind the head + in front of face match the face sprites sprite.
        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].x = playerGraphicsCCGData.BaseHeadSprite.x + offsetFaceAngleForBehindHeadPosX * spritesInfo[i].distanceFromHeadModifier;
            sLeaser.sprites[i].y = playerGraphicsCCGData.BaseHeadSprite.y + offsetFaceAngleForBehindHeadPosY * spritesInfo[i].distanceFromHeadModifier;
            sLeaser.sprites[i].scaleX = playerGraphicsCCGData.BaseHeadSprite.scaleX;
            sLeaser.sprites[i].scaleY = playerGraphicsCCGData.BaseHeadSprite.scaleY;
            sLeaser.sprites[i].rotation = playerGraphicsCCGData.faceRotation;
            sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName(spritesInfo[i].name + playerGraphicsCCGData.faceAngle);
        }
    }
}
