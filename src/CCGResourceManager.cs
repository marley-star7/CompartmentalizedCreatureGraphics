namespace CompartmentalizedCreatureGraphics;

public abstract class CCGResourceManager
{
    public abstract string PropertiesDirectory { get; }

    // You are moving Cosmetic manager stuff to this class to be able to re-use for cosmetic effect manager as well, and load both.
    // Instead of a singleton, you will load an instance of this within the ccg class, which that is loaded (idk whenever lol, likely all the time)

    //
    //-- MS7: The only stuff user has to care about.
    //

    protected readonly static Dictionary<string, Critcos> registeredCritcoses = new();

    public void RegisterCritcos(Critcos critcos)
    {
        if (registeredCritcoses.ContainsKey(critcos.CosmeticTypeID))
        {
            Plugin.LogCCGError($"Critcos with type id {critcos.CosmeticTypeID} is already registered, skipping registration.");
            return;
        }
        registeredCritcoses.Add(critcos.CosmeticTypeID, critcos);
#if DEBUG
        Plugin.LogCCGDebug($"Registered Critcos with name: {critcos.CosmeticTypeID}");
#endif
    }

    public Critcos GetCritcosFromCosmeticTypeId(string cosmeticTypeId)
    {
        cosmeticTypeId = PrepareStringForReference(cosmeticTypeId);

        if (registeredCritcoses.TryGetValue(cosmeticTypeId, out var critcos))
        {
            return critcos;
        }
        else
        {
            Plugin.LogCCGError($"Failed to get Critcos for type id: {cosmeticTypeId}, returning null");
            return null;
        }
    }

    public CCGCosmeticProperties GetLoadedCosmeticPropertiesForId(string cosmeticTypeId, string propertiesId)
    {
        cosmeticTypeId = PrepareStringForReference(cosmeticTypeId);

        if (!registeredCritcoses.TryGetValue(cosmeticTypeId, out var critcos))
        {
            Plugin.LogCCGError($"Failed to get Critcos for type id: {cosmeticTypeId}, returning null");
            return null;
        }
        if (!critcos.TryGetLoadedPropertiesFromPropertiesId(propertiesId, out var properties))
        {
            Plugin.LogCCGError($"Failed to get loaded cosmetic properties for type id: {cosmeticTypeId} and properties id: {propertiesId}, returning null");
            return null;
        }

        return properties;
    }

    /// <summary>
    /// MS7: Get the path to the cosmetic preset name, path is decided directly from the name of the type for consitency.
    /// </summary>
    /// <param name="cosmeticTypeId"></param>
    /// <returns></returns>
    protected string[]? GetDirectoryForCosmeticPropertiesOfCosmeticTypeId(string cosmeticTypeId)
    {
        cosmeticTypeId = PrepareStringForReference(cosmeticTypeId);
        var directory = AssetManager.ListDirectory(PropertiesDirectory + "/" + cosmeticTypeId);

        if (directory == null || directory.Length == 0)
        {
            Plugin.LogCCGError($"Failed to get directory for cosmetic properties of type id: {cosmeticTypeId}, returning");
            return null;
        }

        return directory;
    }

    protected static string GetCosmeticPropertiesIdFromPath(string path)
    {
        // Remove the file extension and return the name.
        return Path.GetFileNameWithoutExtension(path);
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

    private void LoadPropertiesOfCosmeticTypeId(string dynamicCosmeticTypeID)
    {
        dynamicCosmeticTypeID = PrepareStringForReference(dynamicCosmeticTypeID);

#if DEBUG
        Plugin.LogCCGDebug($"Loading cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID}");
#endif

        var directory = GetDirectoryForCosmeticPropertiesOfCosmeticTypeId(dynamicCosmeticTypeID);

        if (directory == null)
        {
            return;
        }

        foreach (string path in directory)
        {
            var propertiesId = GetCosmeticPropertiesIdFromPath(path);
            Plugin.LogCCGInfo($"Loading cosmetic properties id: {propertiesId} at path: {path}");

            var json = File.ReadAllText(path);
            var critcos = GetCritcosFromCosmeticTypeId(dynamicCosmeticTypeID);
            try
            {
                var parsedProperties = critcos.ParseProperties(json);
                if (parsedProperties == null)
                {
                    Plugin.LogCCGError($"Failed to parse properties for cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID} at path: {path}, parsedProperties is null");
                    continue;
                }

                critcos.AddLoadedProperties(propertiesId, parsedProperties);
            }
            catch (Exception e)
            {
                Plugin.LogCCGError($"Failed to load cosmetic properties of type id: {dynamicCosmeticTypeID} at path: {path}, Exception: {e}");
            }
        }
    }

    internal void LoadProperties()
    {
        //-- MS7: Little seperators to help it stand out more in the LogCCG.
        Plugin.LogCCGInfo("//");
        Plugin.LogCCGInfo("//-- Loading CCG cosmetic properties...");
        Plugin.LogCCGInfo("//");

        foreach (var cosmeticType in registeredCritcoses)
            LoadPropertiesOfCosmeticTypeId(cosmeticType.Key);
    }
}
