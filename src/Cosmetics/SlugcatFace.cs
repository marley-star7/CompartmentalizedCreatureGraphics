using UnityEngine;
using RWCustom;

using MRCustom.Math;

namespace CompartmentalizedCreatureGraphics;

public class SlugcatFace : SlugcatCosmetic
{
    /// <summary>
    /// Returns sLeaser.sprites[0]
    /// </summary>
    public FSprite FaceSprite
    {
        get { return sLeaser.sprites[0]; }
        set { sLeaser.sprites[0] = value; }
    }

    public string faceSpriteName;

    public SlugcatFace()
    {

    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[1];
        FaceSprite = new FSprite(faceSpriteName, true);

        AddToContainer(sLeaser, rCam, null);
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        // TODO: make the sideways head turn sprite move two pixels down only when sideways

        if (player == null)
            return;

        var playerGraphics = (PlayerGraphics)player.graphicsModule;
        var playerGraphicsData = playerGraphics.GetPlayerGraphicsCraftingData();

        float lookAllowance = Mathf.Abs(playerGraphics.player.flipDirection + playerGraphics.lookDirection.x) / 2;
        if (lookAllowance > 0.45f)
        {
            FaceSprite.element = Futile.atlasManager.GetElementWithName("crafter" + "FaceNew_EyesOpen2");
            playerGraphics.SetFaceAngle(2);
        }

        else if (lookAllowance > 0.3f)
        {
            FaceSprite.element = Futile.atlasManager.GetElementWithName("crafter" + "FaceNew_EyesOpen1");
            playerGraphics.SetFaceAngle(1);
        }
        else
        {
            FaceSprite.element = Futile.atlasManager.GetElementWithName("crafter" + "FaceNew_EyesOpen0");
            playerGraphics.SetFaceAngle(0);
            FaceSprite.y -= 2;
        }

        //-- MR7: If player is sideways, offset the rotation around the head based how much.
        // This is to face the effect of the head turning sideways on horizontals. 
        Vector2 dirLowerChunkToMainChunk = Custom.DirVec(playerGraphics.player.bodyChunks[1].pos, playerGraphics.player.mainBodyChunk.pos);
        playerGraphicsData.headFaceRotationDegrees = Custom.VecToDeg(dirLowerChunkToMainChunk);
        playerGraphicsData.headFaceRotationDegrees -= dirLowerChunkToMainChunk.x * 90;

        // Snap the rotation and placement so it doens't break at weird spots.
        //faceSprite.x = MarMathf.Snap(faceSprite.x, 1f);
        //faceSprite.y = MarMathf.Snap(faceSprite.y, 1f);
        FaceSprite.rotation = MarMathf.Snap(playerGraphicsData.headFaceRotationDegrees, 5);
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
            FaceSprite.MoveInFrontOfOtherNode(playerGraphicsData.OriginalFaceSprite);
        }
    }
}
