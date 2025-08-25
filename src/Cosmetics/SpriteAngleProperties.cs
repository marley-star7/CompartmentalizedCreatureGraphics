namespace CompartmentalizedCreatureGraphics.Cosmetics;

public struct SpriteAngleProperties
{
    /// <summary>
    /// The default position set for the cosmetic at each angle.
    /// All calculations are done with half the length treated as the center value.
    /// Currently only supports up to two index in either direction.
    /// </summary>
    [JsonProperty("positions")]
    [JsonConverter(typeof(Vector2ArrayJsonConverter))]
    public Vector2[] positions = new Vector2[] { Vector2.zero };

    public SpriteAngleProperties(Vector2[] vector2s)
    {
        this.positions = vector2s;
    }
}
