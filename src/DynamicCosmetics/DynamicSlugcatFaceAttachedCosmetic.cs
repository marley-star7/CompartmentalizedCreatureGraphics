using UnityEngine;

namespace CompartmentalizedCreatureGraphics.SlugcatCosmetics;

public class DynamicSlugcatFaceAttachedCosmetic : DynamicSlugcatCosmetic
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

    public DynamicSlugcatFaceAttachedCosmetic()
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
        var playerGraphicsCCGData = playerGraphics.GetPlayerGraphicsCCGData();

        if (playerGraphicsCCGData.sLeaser == null)
            return;

        //-- MR7: To achieve the effect of being behind we make get an offset from face angle different to position the head.
        var offsetFaceAngleForBehindHeadPosX = playerGraphicsCCGData.OriginalFaceSprite.x - playerGraphicsCCGData.OriginalHeadSprite.x;
        var offsetFaceAngleForBehindHeadPosY = playerGraphicsCCGData.OriginalFaceSprite.y - playerGraphicsCCGData.OriginalHeadSprite.y;

        //-- Loop through and update all sprites behind the head + in front of face match the face sprites sprite.
        for (int i = 0; i < behindHeadSprites.Length; i++)
        {
            sLeaser.sprites[i].x = playerGraphicsCCGData.OriginalHeadSprite.x - offsetFaceAngleForBehindHeadPosX * behindHeadSprites[i].distanceFromHeadModifier;
            sLeaser.sprites[i].y = playerGraphicsCCGData.OriginalHeadSprite.y - offsetFaceAngleForBehindHeadPosY * behindHeadSprites[i].distanceFromHeadModifier;
            sLeaser.sprites[i].scaleX = playerGraphicsCCGData.OriginalHeadSprite.scaleX;
            sLeaser.sprites[i].scaleY = playerGraphicsCCGData.OriginalHeadSprite.scaleY;
            sLeaser.sprites[i].rotation = playerGraphicsCCGData.faceRotation;
            sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName(behindHeadSprites[i].name + playerGraphicsCCGData.faceAngle);
            sLeaser.sprites[i].color = behindHeadSprites[i].Color();
        }

        for (int i = 0; i < inFrontOfFaceSprites.Length; i++)
        {
            var currentSprite = sLeaser.sprites[behindHeadSprites.Length + i];

            currentSprite.x = playerGraphicsCCGData.OriginalHeadSprite.x + offsetFaceAngleForBehindHeadPosX * inFrontOfFaceSprites[i].distanceFromHeadModifier;
            currentSprite.y = playerGraphicsCCGData.OriginalHeadSprite.y + offsetFaceAngleForBehindHeadPosY * inFrontOfFaceSprites[i].distanceFromHeadModifier;
            currentSprite.scaleX = playerGraphicsCCGData.OriginalHeadSprite.scaleX;
            currentSprite.scaleY = playerGraphicsCCGData.OriginalHeadSprite.scaleY;
            currentSprite.rotation = playerGraphicsCCGData.faceRotation;
            currentSprite.element = Futile.atlasManager.GetElementWithName(inFrontOfFaceSprites[i].name + playerGraphicsCCGData.faceAngle);
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

        var playerGraphicsCCGData = ((PlayerGraphics)player.graphicsModule).GetPlayerGraphicsCCGData();
        if (playerGraphicsCCGData.sLeaser != null)
        {
            // Order of sprites positioning in front of each other is through the array back to front.

            sLeaser.sprites[0].MoveBehindOtherNode(playerGraphicsCCGData.OriginalHeadSprite);
            for (int i = 1; i < behindHeadSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveBehindOtherNode(sLeaser.sprites[i - 1]);
            }

            sLeaser.sprites[0].MoveInFrontOfOtherNode(playerGraphicsCCGData.dynamicCosmetics[playerGraphicsCCGData.dynamicCosmetics.Count - 2].LastSprite);
            for (int i = 1; i < behindHeadSprites.Length + inFrontOfFaceSprites.Length; i++)
            {
                sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
            }
        }
    }
}
