namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatCosmetic : DynamicCreatureCosmetic
{
    public new class Properties : DynamicCreatureCosmetic.Properties
    {
        [JsonProperty("spriteLayerGroups")]
        [JsonConverter(typeof(SpriteLayerGroupArrayConverter<Enums.SlugcatCosmeticLayer>))]
        protected override SpriteLayerGroup[] SpriteLayerGroupsSetter
        {
            set => spriteLayerGroups = value;
        }

        public override DynamicCreatureCosmetic.Properties Parse(string json)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };

            return JsonConvert.DeserializeObject<Properties>(json, settings);
        }
    }

    public Player player => (Player)Wearer;

    public DynamicSlugcatCosmetic(PlayerGraphics playerGraphics, DynamicCreatureCosmetic.Properties properties) : base(playerGraphics, properties)
    {
    }
}
