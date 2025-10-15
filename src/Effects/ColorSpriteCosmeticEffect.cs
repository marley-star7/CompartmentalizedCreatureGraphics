
namespace CompartmentalizedCreatureGraphics.Effects;

public class ColorSpriteCosmeticEffect : DynamicCosmeticEffect
{
    public new class Properties : DynamicCosmeticEffect.Properties
    {
        [JsonConverter(typeof(HexadacimalToUnityColorConverter))]
        [JsonProperty("color")]
        public Color color = Color.white;
    }

    public new Properties properties => (Properties)_properties;

    public ColorSpriteCosmeticEffect(IDynamicCreatureCosmetic cosmetic, byte spriteEffectGroup) : base(cosmetic, spriteEffectGroup)
    {

    }

    public override void OnCosmeticUpdatePalette(RoomCamera.SpriteLeaser cosmeticSLeaser, RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette)
    {
        var spritesToEffect = cosmetic.SpriteEffectGroups[spriteEffectGroup].Sprites;

        for (int i = 0; i < spritesToEffect.Length; i++)
        {
            cosmeticSLeaser.sprites[spritesToEffect[i]].color = properties.color;
        }
    }
}
