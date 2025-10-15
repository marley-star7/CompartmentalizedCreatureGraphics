namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatTailCirclesCosmetic : DynamicSlugcatCosmetic
{
    public new class Properties : DynamicSlugcatCosmetic.Properties
    {
        [JsonConverter(typeof(HexadacimalToUnityColorConverter))]
        [JsonProperty("color")]
        public Color color = Color.white;

        [JsonProperty("bigCircles")]
        public bool bigCircles = true;

        [JsonProperty("rows")]
        public int rows = 5;

        [JsonProperty("lines")]
        public int lines = 3;

        public override DynamicCreatureCosmetic.Properties Parse(string json)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };

            return JsonConvert.DeserializeObject<Properties>(json, settings);
        }
    }

    public Properties properties => (Properties)_properties;

    public int NumberOfSprites => properties.rows * properties.lines + 1;

    public float spearProg = 0f;

    public int spearLine;

    public int spearRow;

    public int spearType;

    public DynamicSlugcatTailCirclesCosmetic(PlayerGraphics playerGraphics, Properties properties) : base(playerGraphics, properties)
    {
    }

    public float GetCirclePosFactor(int row, int line)
    {
        float lineNum = ((float)line + ((row % 2 != 0) ? 0f : 0.5f)) / (float)(properties.lines - 1);
        lineNum = -1f + 2f * lineNum;
        if (lineNum < -1f)
        {
            lineNum += 2f;
        }
        else if (lineNum > 1f)
        {
            lineNum -= 2f;
        }

        return lineNum = Mathf.Sign(lineNum) * Mathf.Pow(Mathf.Abs(lineNum), 0.6f);
    }
}
