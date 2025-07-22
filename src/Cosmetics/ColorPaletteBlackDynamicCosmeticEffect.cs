using UnityEngine;

namespace CompartmentalizedCreatureGraphics;

// TODO: set this up as a resource, in that it is a static class with a swapback array, that runs the code for every function of it's kind real quickly.
// TODO: basically like a shader.
public class PaletteColorizeDynamicCosmeticEffect : DynamicCosmeticEffect
{
    public Color lerpToFromBlackColor = Color.white;
    public float lerpAmount = 0;

    public override void OnCosmeticUpdatePalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {
        var colorToSet = Color.Lerp(palette.blackColor, lerpToFromBlackColor, lerpAmount);

        for (int i = 0; i < wearerSLeaser.sprites.Length; i++)
        {
            wearerSLeaser.sprites[i].color = colorToSet;
        }
    }
}
