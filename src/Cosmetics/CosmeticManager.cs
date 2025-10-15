namespace CompartmentalizedCreatureGraphics.Cosmetics;

public class CosmeticManager : CCGResourceManager
{
    public override string PropertiesDirectory => Plugin.CosmeticPropertiesDirectory;

    //
    //-- MS7: The only stuff user has to care about.
    //

    private readonly Dictionary<string, SpriteAngleProperties> loadedSpriteAngleProperties = new();

    public SpriteAngleProperties GetSpriteAnglePropertiesForId(string spriteAnglePropertiesId)
    {
        spriteAnglePropertiesId = PrepareStringForReference(spriteAnglePropertiesId);

        if (loadedSpriteAngleProperties.TryGetValue(spriteAnglePropertiesId, out var foundSpriteAngleProperties))
        {
            return foundSpriteAngleProperties;
        }

        Plugin.LogCCGError($"Failed to get sprite angle properties for id: {spriteAnglePropertiesId}, returning default (single 0,0 position)");
        return new SpriteAngleProperties(new Vector2[] {Vector2.zero});
    }

    private string GetSpriteAnglePropertiesIdFromPath(string path)
    {
        // Remove the file extension and return the name.
        return Path.GetFileNameWithoutExtension(path);
    }

    private SpriteAngleProperties ParseSpriteAngleProperties(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Error,
        };

        return JsonConvert.DeserializeObject<SpriteAngleProperties>(json, settings);
    }

    private void AddLoadedSpriteAngleProperties(string id, SpriteAngleProperties properties)
    {
        id = PrepareStringForReference(id);

        if (loadedSpriteAngleProperties.ContainsKey(id))
        {
            Plugin.LogCCGWarning($"Sprite angle properties with ID: {id} is already loaded, skipping addition.");
            return;
        }
        loadedSpriteAngleProperties.Add(id, properties);
    }

    internal void LoadSpriteAngleProperties()
    {
        Plugin.LogCCGInfo("//");
        Plugin.LogCCGInfo("//-- Loading CCG sprite angle properties...");
        Plugin.LogCCGInfo("//");

        var directory = AssetManager.ListDirectory(Plugin.SpriteAnglePropertiesDirectory);

        foreach (string path in directory)
        {
            var id = GetSpriteAnglePropertiesIdFromPath(path);
            Plugin.LogCCGInfo($"Loading sprite angle properties id {id} at path: {path}");

            var json = File.ReadAllText(path);
            var parsedSpriteAngleProperties = ParseSpriteAngleProperties(json);
            AddLoadedSpriteAngleProperties(id, parsedSpriteAngleProperties);
        }
    }
}
