using CompartmentalizedCreatureGraphics.Extensions;
using UnityEngine;

namespace CompartmentalizedCreatureGraphics.SlugcatCosmetics;

public class DynamicSlugcatFullHeadAttachedCosmetic : DynamicSlugcatCosmetic
{
    public struct SpriteLayer
    {
        public string name;
        public float distanceFromHeadModifier = 1f;
        public Color color;

        public SpriteLayer(string name)
        {
            this.name = name;
        }
    }

    public int totalSpritesLength
    {
        get { return behindHeadSprites.Length + inFrontOfHeadSprites.Length + inFrontOfFaceSprites.Length; }
    }

    public SpriteLayer[] behindHeadSprites;
    public SpriteLayer[] inFrontOfHeadSprites;
    public SpriteLayer[] inFrontOfFaceSprites;

    public int startOfBehindHeadSprites
    {
        get => 0;
    }
    public int startOfInFrontOfHeadSprites
    {
        get => behindHeadSprites.Length;
    }
    public int startOfInFrontOfFaceSprites
    {
        get => behindHeadSprites.Length + inFrontOfHeadSprites.Length;
    }

    public DynamicSlugcatFullHeadAttachedCosmetic()
    {
        this.behindHeadSprites = new SpriteLayer[0];
        this.inFrontOfHeadSprites = new SpriteLayer[0];
        this.inFrontOfFaceSprites = new SpriteLayer[0];
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[totalSpritesLength];

        for (int i = 0; i < behindHeadSprites.Length; i++)
        {
            var currentIndex = i + startOfBehindHeadSprites;
            sLeaser.sprites[currentIndex] = new FSprite(behindHeadSprites[i].name + "A0", true);
        }
        for (int i = 0; i < inFrontOfHeadSprites.Length; i++)
        {
            var currentIndex = i + startOfInFrontOfHeadSprites;
            sLeaser.sprites[currentIndex] = new FSprite(inFrontOfHeadSprites[i].name + "A0", true);
        }
        for (int i = 0; i < inFrontOfFaceSprites.Length; i++)
        {
            var currentIndex = i + startOfInFrontOfFaceSprites;
            sLeaser.sprites[currentIndex] = new FSprite(inFrontOfFaceSprites[i].name + "A0", true);
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
        var offsetFaceAngleForBehindHeadPosX = playerGraphicsCCGData.OriginalFaceSprite.x - playerGraphicsCCGData.OriginalHeadSprite.x;
        var offsetFaceAngleForBehindHeadPosY = playerGraphicsCCGData.OriginalFaceSprite.y - playerGraphicsCCGData.OriginalHeadSprite.y;

        //-- Loop through and update all sprites behind the head + in front of face match the face sprites sprite.
        for (int i = 0; i < behindHeadSprites.Length; i++)
        {
            sLeaser.sprites[i].x = playerGraphicsCCGData.OriginalHeadSprite.x - (offsetFaceAngleForBehindHeadPosX * behindHeadSprites[i].distanceFromHeadModifier);
            sLeaser.sprites[i].y = playerGraphicsCCGData.OriginalHeadSprite.y - (offsetFaceAngleForBehindHeadPosY * behindHeadSprites[i].distanceFromHeadModifier);
            sLeaser.sprites[i].scaleX = playerGraphicsCCGData.OriginalHeadSprite.scaleX;
            sLeaser.sprites[i].scaleY = playerGraphicsCCGData.OriginalHeadSprite.scaleY;
            sLeaser.sprites[i].rotation = playerGraphicsCCGData.faceRotation;
            sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName(behindHeadSprites[i].name + playerGraphicsCCGData.faceAngle);
            sLeaser.sprites[i].color = behindHeadSprites[i].color;
        }

        for (int i = 0; i < inFrontOfHeadSprites.Length; i++)
        {
            var currentSprite = sLeaser.sprites[startOfInFrontOfHeadSprites + i];

            currentSprite.x = playerGraphicsCCGData.OriginalHeadSprite.x + (offsetFaceAngleForBehindHeadPosX * inFrontOfHeadSprites[i].distanceFromHeadModifier);
            currentSprite.y = playerGraphicsCCGData.OriginalHeadSprite.y + (offsetFaceAngleForBehindHeadPosY * inFrontOfHeadSprites[i].distanceFromHeadModifier);
            currentSprite.scaleX = playerGraphicsCCGData.OriginalHeadSprite.scaleX;
            currentSprite.scaleY = playerGraphicsCCGData.OriginalHeadSprite.scaleY;
            currentSprite.rotation = playerGraphicsCCGData.faceRotation;
            currentSprite.element = Futile.atlasManager.GetElementWithName(inFrontOfHeadSprites[i].name + playerGraphicsCCGData.faceAngle);
            currentSprite.color = inFrontOfHeadSprites[i].color;
        }

        for (int i = 0; i < inFrontOfFaceSprites.Length; i++)
        {
            var currentSprite = sLeaser.sprites[startOfInFrontOfFaceSprites + i];

            currentSprite.x = playerGraphicsCCGData.OriginalHeadSprite.x + (offsetFaceAngleForBehindHeadPosX * inFrontOfFaceSprites[i].distanceFromHeadModifier);
            currentSprite.y = playerGraphicsCCGData.OriginalHeadSprite.y + (offsetFaceAngleForBehindHeadPosY * inFrontOfFaceSprites[i].distanceFromHeadModifier);
            currentSprite.scaleX = playerGraphicsCCGData.OriginalHeadSprite.scaleX;
            currentSprite.scaleY = playerGraphicsCCGData.OriginalHeadSprite.scaleY;
            currentSprite.rotation = playerGraphicsCCGData.faceRotation;
            currentSprite.element = Futile.atlasManager.GetElementWithName(inFrontOfFaceSprites[i].name + playerGraphicsCCGData.faceAngle);
            currentSprite.color = inFrontOfFaceSprites[i].color;
        }
    }

    public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {

    }

    //-- MR7: Change to somehow have internal containers on the playergraphics, which these are added into?

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

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();
        if (playerGraphicsCCGData.sLeaser != null)
        {
            // Order of sprites positioning in front of each other is through the array back to front.

            sLeaser.sprites[startOfBehindHeadSprites].MoveBehindOtherNode(playerGraphicsCCGData.OriginalHeadSprite);
            for (int i = 1 + startOfBehindHeadSprites; i < startOfBehindHeadSprites + behindHeadSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveBehindOtherNode(sLeaser.sprites[i - 1]);
            }

            sLeaser.sprites[startOfInFrontOfHeadSprites].MoveBehindOtherNode(playerGraphicsCCGData.OriginalFaceSprite);
            for (int i = 1 + startOfInFrontOfHeadSprites; i < startOfInFrontOfHeadSprites + inFrontOfHeadSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
            }

            sLeaser.sprites[startOfInFrontOfFaceSprites].MoveInFrontOfOtherNode(playerGraphicsCCGData.dynamicCosmetics[playerGraphicsCCGData.dynamicCosmetics.Count - 2].LastSprite);
            for (int i = 1 + startOfInFrontOfFaceSprites; i < startOfInFrontOfFaceSprites + inFrontOfFaceSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
            }
        }
    }
}
