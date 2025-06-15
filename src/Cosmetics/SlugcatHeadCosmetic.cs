using UnityEngine;

namespace CompartmentalizedCreatureGraphics;

public class SlugcatHeadCosmetic : SlugcatCosmetic
{
    public int totalSpritesLength
    {
        get { return inFrontOfHeadSpritesNames.Length + behindHeadSpritesNames.Length; }
    }

    public string[] behindHeadSpritesNames;
    public string[] inFrontOfHeadSpritesNames;

    public SlugcatHeadCosmetic()
    {
        this.behindHeadSpritesNames = new string[0];
        this.inFrontOfHeadSpritesNames = new string[0];
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[totalSpritesLength];

        for (int i = 0; i < behindHeadSpritesNames.Length; i++)
        {
            sLeaser.sprites[i] = new FSprite((behindHeadSpritesNames[i] + "HeadA0"),true);
        }
        for (int i = 0; i < inFrontOfHeadSpritesNames.Length; i++)
        {
            sLeaser.sprites[behindHeadSpritesNames.Length + i] = new FSprite(inFrontOfHeadSpritesNames[i] + "HeadA0", true);
        }

        AddToContainer(sLeaser, rCam, null);
    }

    public override void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (player == null)
            return;

        var graphicsModuleData = ((PlayerGraphics)player.graphicsModule).GetGraphicsModuleCraftingData();

        //-- No sLeaser made yet, return.
        if (graphicsModuleData.sLeaser == null)
            return;

        FSprite playerHeadSprite = graphicsModuleData.sLeaser.sprites[3];

        //-- Make all sprites mimick the position and rotation of the playerHeadSprite
        for (int i = 0; i < sLeaser.sprites.Length; i++)
        {
            sLeaser.sprites[i].x = playerHeadSprite.x;
            sLeaser.sprites[i].y = playerHeadSprite.y;
            sLeaser.sprites[i].rotation = playerHeadSprite.rotation;
        }

        //-- Loop through and update all sprites behind the head + in front to match the head sprites sprite.
        for (int i = 0; i < behindHeadSpritesNames.Length; i++)
        {
            sLeaser.sprites[i].element = Futile.atlasManager.GetElementWithName((behindHeadSpritesNames[i] + playerHeadSprite.element.name));
        }

        for (int i = 0; i < inFrontOfHeadSpritesNames.Length; i++)
        {
            sLeaser.sprites[behindHeadSpritesNames.Length + i].element = Futile.atlasManager.GetElementWithName(inFrontOfHeadSpritesNames[i] + playerHeadSprite.element.name);
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

        var playerGraphicsData = ((PlayerGraphics)player.graphicsModule).GetGraphicsModuleCraftingData();
        if (playerGraphicsData.sLeaser != null)
        {
            FSprite playerHeadSprite = playerGraphicsData.sLeaser.sprites[3];

            // First (behind/front) sprite is positioned directly (behind/in front) of the player head sprite.
            // Meanwhile continued sprites in the array are positioned (behind/in front of) their previous.

            sLeaser.sprites[0].MoveBehindOtherNode(playerHeadSprite);
            for (int i = 1; i < behindHeadSpritesNames.Length; i++)
            {
                sLeaser.sprites[i].MoveBehindOtherNode(sLeaser.sprites[i - 1]);
            }

            sLeaser.sprites[0].MoveInFrontOfOtherNode(playerHeadSprite);
            for (int i = 1; i < behindHeadSpritesNames.Length + inFrontOfHeadSpritesNames.Length; i++)
            {
                sLeaser.sprites[i].MoveInFrontOfOtherNode(sLeaser.sprites[i - 1]);
            }
        }
    }
}
