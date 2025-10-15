namespace CompartmentalizedCreatureGraphics.Cosmetics;

public interface IModifySpriteLeaser
{
    public void PostWearerInitiateSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam);

    public void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos);

    /// <summary>
    /// -- MS7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    /// </summary>
    /// <param name="wearerSLeaser"></param>
    /// <param name="rCam"></param>
    /// <param name="palette"></param>
    public void PostWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette);
}
