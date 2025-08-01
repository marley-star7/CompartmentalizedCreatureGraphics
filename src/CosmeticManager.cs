using System.IO;

namespace CompartmentalizedCreatureGraphics;

public static class CosmeticManager
{
    //
    //-- MR7: The only stuff user has to care about.
    //

    private readonly static Dictionary<string, Critcos> registeredCritcoses = new();

    public static void RegisterCritcos(Critcos critcos)
    {
        if (registeredCritcoses.ContainsKey(critcos.CosmeticTypeID))
        {
            Plugin.LogError($"Critcos with type id {critcos.CosmeticTypeID} is already registered, skipping registration.");
            return;
        }
        registeredCritcoses.Add(critcos.CosmeticTypeID, critcos);
        Plugin.LogDebug($"Registered Critcos with name: {critcos.CosmeticTypeID}");
    }

    //
    //-- MR7: And everything else lol...
    //

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
        //-- MR7: Get the path to the cosmetic preset name, path is decided directly from the name of the type for consitency.
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

    private static void LoadPropertiesOfCosmeticTypeId(string dynamicCosmeticTypeID)
    {
        dynamicCosmeticTypeID = PrepareStringForReference(dynamicCosmeticTypeID);

        Plugin.LogDebug($"Loading cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID}");

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

                Plugin.LogDebug($"Loading cosmetic properties id: {propertiesId} at path: {path}");

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
    /// MR7: Prepare the name for cosmetic reference, this is just a simple method to ensure that the name is lowercase.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static string PrepareStringForReference(string name)
    {
        return name.ToLowerInvariant();
    }

    internal static void LoadCosmeticProperties()
    {
        //-- MR7: Little seperators to help it stand out ore in the log.
        Plugin.LogDebug("//");
        Plugin.LogDebug("//-- Loading cosmetic properties...");
        Plugin.LogDebug("//");

        foreach (var cosmeticType in registeredCritcoses)
            LoadPropertiesOfCosmeticTypeId(cosmeticType.Key);

        Plugin.LogDebug("//");
        Plugin.LogDebug("//-- Finished loading cosmetic properties.");
        Plugin.LogDebug("//");
    }
}
