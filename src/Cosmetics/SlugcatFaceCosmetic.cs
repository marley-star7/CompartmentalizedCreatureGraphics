using UnityEngine;

namespace CompartmentalizedCreatureGraphics;

public class SlugcatFaceCosmetic : SlugcatCosmetic
{
    public struct FaceSpriteCosmetic
    {
        public delegate Color GetColor();

        public string name;
        public float distanceFromHeadModifier = 1f;
        public GetColor Color;

        public FaceSpriteCosmetic(string name)
        {
            this.name = name;
        }
    }

    public int totalSpritesLength
    {
        get { return inFrontOfFaceSprites.Length + behindHeadSprites.Length; }
    }

    //-- MR7: Can likely just extend face sprite cosmetic to tell what node it will be behind and infront instead of this.
    public FaceSpriteCosmetic[] behindHeadSprites;
    public FaceSpriteCosmetic[] inFrontOfFaceSprites;

    public SlugcatFaceCosmetic()
    {
        this.behindHeadSprites = new FaceSpriteCosmetic[0];
        this.inFrontOfFaceSprites = new FaceSpriteCosmetic[0];
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[totalSpritesLength];

        for (int i = 0; i < behindHeadSprites.Length; i++)
        {
            sLeaser.sprites[i] = new FSprite((behindHeadSprites[i].name + "A0"), true);
        }
        /* // TODO: add
        for (int i = 0; i < behindFaceSprites.Length; i++)
        {
            sLeaser.sprites[behindHeadSprites.Length + i] = new FSprite(inFrontOfFaceSprites[i].name + "FaceA0", true);
        }
        */
        for (int i = 0; i < inFrontOfFaceSprites.Length; i++)
        {
            sLeaser.sprites[behindHeadSprites.Length + i] = new FSprite(inFrontOfFaceSprites[i].name + "A0", true);
        }

        AddToContainer(sLeaser, rCam, null);
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

        var noEmotionBaseFaceSpriteAngle = playerGraphicsData.FaceAngle;
        //-- Loop through and update all sprites behind the head + in front of face match the face sprites sprite.
        for (int i = 0; i < behindHeadSprites.Length; i++)
        {
            sLeaser.sprites[i].x = playerGraphicsData.HeadSprite.x - offsetFaceAngleForBehindHeadPosX * behindHeadSprites[i].distanceFromHeadModifier;
            sLeaser.sprites[i].y = playerGraphicsData.HeadSprite.y - offsetFaceAngleForBehindHeadPosY * behindHeadSprites[i].distanceFromHeadModifier;
            sLeaser.sprites[i].scaleX = playerGraphicsData.HeadSprite.scaleX;
            sLeaser.sprites[i].scaleY = playerGraphicsData.HeadSprite.scaleY;
            sLeaser.sprites[i].rotation = playerGraphicsData.OriginalFaceSprite.rotation;
            sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName(behindHeadSprites[i].name + noEmotionBaseFaceSpriteAngle);
            sLeaser.sprites[i].color = behindHeadSprites[i].Color();
        }

        for (int i = 0; i < inFrontOfFaceSprites.Length; i++)
        {
            var currentSprite = sLeaser.sprites[behindHeadSprites.Length + i];

            currentSprite.x = playerGraphicsData.HeadSprite.x + offsetFaceAngleForBehindHeadPosX * inFrontOfFaceSprites[i].distanceFromHeadModifier;
            currentSprite.y = playerGraphicsData.HeadSprite.y + offsetFaceAngleForBehindHeadPosY * inFrontOfFaceSprites[i].distanceFromHeadModifier;
            currentSprite.scaleX = playerGraphicsData.HeadSprite.scaleX;
            currentSprite.scaleY = playerGraphicsData.HeadSprite.scaleY;
            currentSprite.rotation = playerGraphicsData.OriginalFaceSprite.rotation;
            currentSprite.element = Futile.atlasManager.GetElementWithName(inFrontOfFaceSprites[i].name + noEmotionBaseFaceSpriteAngle);
            currentSprite.color = inFrontOfFaceSprites[i].Color();
        }
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
            // Order of sprites positioning in front of each other is through the array back to front.

            sLeaser.sprites[0].MoveBehindOtherNode(playerGraphicsData.HeadSprite);
            for (int i = 1; i < behindHeadSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveBehindOtherNode(sLeaser.sprites[i - 1]);
            }

            sLeaser.sprites[0].MoveInFrontOfOtherNode(playerGraphicsData.OriginalFaceSprite);
            for (int i = 1; i < behindHeadSprites.Length + inFrontOfFaceSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
            }
        }
    }
}
