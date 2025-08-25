using System.IO;
using System.Runtime;

namespace CompartmentalizedCreatureGraphics;

public static class CosmeticManager
{
    //
    //-- MS7: The only stuff user has to care about.
    //

    private readonly static Dictionary<string, Critcos> registeredCritcoses = new();

    private readonly static Dictionary<string, SpriteAngleProperties> loadedSpriteAngleProperties = new();

    public static void RegisterCritcos(Critcos critcos)
    {
        if (registeredCritcoses.ContainsKey(critcos.CosmeticTypeID))
        {
            Plugin.LogError($"Critcos with type id {critcos.CosmeticTypeID} is already registered, skipping registration.");
            return;
        }
        registeredCritcoses.Add(critcos.CosmeticTypeID, critcos);
#if DEBUG
        Plugin.LogDebug($"Registered Critcos with name: {critcos.CosmeticTypeID}");
#endif
    }

    public static SpriteAngleProperties GetSpriteAnglePropertiesForId(string spriteAnglePropertiesId)
    {
        spriteAnglePropertiesId = PrepareStringForReference(spriteAnglePropertiesId);

        if (loadedSpriteAngleProperties.TryGetValue(spriteAnglePropertiesId, out var foundSpriteAngleProperties))
        {
            return foundSpriteAngleProperties;
        }

        Plugin.LogError($"Failed to get sprite angle properties for id: {spriteAnglePropertiesId}, returning default (single 0,0 position)");
        return new SpriteAngleProperties(new Vector2[] {Vector2.zero});
    }

    public static Critcos GetCritcosFromCosmeticTypeId(string cosmeticTypeId)
    {
        cosmeticTypeId = PrepareStringForReference(cosmeticTypeId);

        if (registeredCritcoses.TryGetValue(cosmeticTypeId, out var critcos))
        {
            return critcos;
        }
        else
        {
            Plugin.LogError($"Failed to get Critcos for type id: {cosmeticTypeId}, returning null");
            return null;
        }
    }

    public static DynamicCreatureCosmetic.Properties GetLoadedCosmeticPropertiesForId(string cosmeticTypeId, string propertiesId)
    {
        cosmeticTypeId = PrepareStringForReference(cosmeticTypeId);

        if(!registeredCritcoses.TryGetValue(cosmeticTypeId, out var critcos))
        {
            Plugin.LogError($"Failed to get Critcos for type id: {cosmeticTypeId}, returning null");
            return null;
        }
        if(!critcos.TryGetLoadedPropertiesFromPropertiesId(propertiesId, out var properties))
        {
            Plugin.LogError($"Failed to get loaded cosmetic properties for type id: {cosmeticTypeId} and properties id: {propertiesId}, returning null");
            return null;
        }

        return properties;
    }

    private static string[]? GetDirectoryForCosmeticPropertiesOfCosmeticTypeId(string cosmeticTypeId)
    {
        cosmeticTypeId = PrepareStringForReference(cosmeticTypeId);
        //-- MS7: Get the path to the cosmetic preset name, path is decided directly from the name of the type for consitency.
        var directory = AssetManager.ListDirectory(Plugin.CosmeticPropertiesDirectory + "/" + cosmeticTypeId);

        if (directory == null || directory.Length == 0)
            return null;
        else
            return directory;
    }

    private static string GetCosmeticPropertiesIdFromPath(string path)
    {
        // Remove the file extension and return the name.
        return Path.GetFileNameWithoutExtension(path);
    }

    private static string GetSpriteAnglePropertiesIdFromPath(string path)
    {
        // Remove the file extension and return the name.
        return Path.GetFileNameWithoutExtension(path);
    }

    private static SpriteAngleProperties ParseSpriteAngleProperties(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Error,
        };

        return JsonConvert.DeserializeObject<SpriteAngleProperties>(json, settings);
    }

    private static void AddLoadedSpriteAngleProperties(string id, SpriteAngleProperties properties)
    {
        id = PrepareStringForReference(id);

        if (loadedSpriteAngleProperties.ContainsKey(id))
        {
            Plugin.LogWarning($"Sprite angle properties with ID: {id} is already loaded, skipping addition.");
            return;
        }
        loadedSpriteAngleProperties.Add(id, properties);
    }

    internal static void LoadSpriteAngleProperties()
    {
        Plugin.LogInfo("//");
        Plugin.LogInfo("//-- Loading CCG sprite angle properties...");
        Plugin.LogInfo("//");

        var directory = AssetManager.ListDirectory(Plugin.SpriteAnglePropertiesDirectory);

        foreach (string path in directory)
        {
            var id = GetSpriteAnglePropertiesIdFromPath(path);
            Plugin.LogInfo($"Loading sprite angle properties id {id} at path: {path}");

            var json = File.ReadAllText(path);
            var parsedSpriteAngleProperties = ParseSpriteAngleProperties(json);
            AddLoadedSpriteAngleProperties(id, parsedSpriteAngleProperties);
        }
    }

    private static void LoadPropertiesOfCosmeticTypeId(string dynamicCosmeticTypeID)
    {
        dynamicCosmeticTypeID = PrepareStringForReference(dynamicCosmeticTypeID);

#if DEBUG
        Plugin.LogDebug($"Loading cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID}");
#endif

        try
        {
            var directory = GetDirectoryForCosmeticPropertiesOfCosmeticTypeId(dynamicCosmeticTypeID);

            if (directory == null)
            {
                Plugin.LogError($"Failed to get directory for cosmetic properties of type id: {dynamicCosmeticTypeID}, returning");
                return;
            }

            foreach (string path in directory)
            {
                var propertiesId = GetCosmeticPropertiesIdFromPath(path);
                Plugin.LogInfo($"Loading cosmetic properties id: {propertiesId} at path: {path}");

                /*
                var jsonData = Json.Parser.Parse(File.ReadAllText(path)) as Dictionary<string, object>;
                if (jsonData == null)
                {
                    Plugin.LogError($"Failed to parse JSON data for cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID} at path: {path}, JSON Data is null");
                    continue;
                }
                */
                var json = File.ReadAllText(path);
                var critcos = GetCritcosFromCosmeticTypeId(dynamicCosmeticTypeID);
                var parsedProperties = critcos.ParseProperties(json);
                if (parsedProperties == null)
                {
                    Plugin.LogError($"Failed to parse properties for cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID} at path: {path}, parsedProperties is null");
                    continue;
                }

                critcos.AddLoadedDynamicCosmeticProperties(propertiesId, parsedProperties);
            }
        }
        catch (Exception e)
        {
            Plugin.LogError($"Failed to load cosmetic properties of type id: {dynamicCosmeticTypeID}, Exception: {e}");
        }
    }

    /// <summary>
    /// MS7: Prepare the name for cosmetic reference, this is just a simple method to ensure that the name is lowercase.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static string PrepareStringForReference(string name)
    {
        return name.ToLowerInvariant();
    }

    internal static void LoadCosmeticProperties()
    {
        //-- MS7: Little seperators to help it stand out more in the log.
        Plugin.LogInfo("//");
        Plugin.LogInfo("//-- Loading CCG cosmetic properties...");
        Plugin.LogInfo("//");

        foreach (var cosmeticType in registeredCritcoses)
            LoadPropertiesOfCosmeticTypeId(cosmeticType.Key);
    }
}
