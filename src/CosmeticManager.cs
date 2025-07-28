using System.IO;

namespace CompartmentalizedCreatureGraphics;

public static class CosmeticManager
{
    private readonly static Dictionary<string, Critcos> registeredCritCoses = new();

    /// <summary>
    /// MR7: Prepare the name for cosmetic reference, this is just a simple method to ensure that the name is lowercase.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string PrepareStringForReference(string name)
    {
        return name.ToLowerInvariant();
    }
    /// <summary>
    /// Normalize the name to lowercase for consistency, so user's don't have to worry about case sensitivity when referencing cosmetics.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string ConvertCosmeticTypeToID(Type cosmeticType)
    {
        return cosmeticType.Name.ToLowerInvariant();
    }

    public static string GetCosmeticTypeIDFromType<T>() where T : DynamicCreatureCosmetic
    {
        Type cosmeticType = typeof(T);
        return GetCosmeticTypeIDFromType(cosmeticType);
    }

    public static string GetCosmeticTypeIDFromType(Type cosmeticType)
    {
        string cosmeticTypeID = ConvertCosmeticTypeToID(cosmeticType);

        if (!dynamicCosmeticTypes.ContainsKey(cosmeticTypeID))
        {
            Plugin.LogError($"Dynamic cosmetic type id: {cosmeticTypeID} is not registered, cannot get!");
            return null;
        }
        return cosmeticTypeID;
    }

    public static DynamicCosmeticTypeInfo? GetCosmeticTypeInfoFromCosmeticTypeID(string cosmeticTypeID)
    {
        cosmeticTypeID = PrepareStringForReference(cosmeticTypeID);

        if (!dynamicCosmeticTypes.TryGetValue(cosmeticTypeID, out var typeInfo))
        {
            Plugin.LogError($"Dynamic cosmetic type id: {cosmeticTypeID} is not registered, cannot get type!");
            return null;
        }
        return typeInfo;
    }

    public static Type GetCosmeticTypeFromCosmeticTypeID(string cosmeticTypeID)
    {
        return GetCosmeticTypeInfoFromCosmeticTypeID(cosmeticTypeID)?.CosmeticType;
    }

    public static Type GetCosmeticPropertiesTypeFromCosmeticTypeID(string cosmeticTypeID)
    {
        return GetCosmeticTypeInfoFromCosmeticTypeID(cosmeticTypeID)?.PropertiesType;
    }

    /// <summary>
    /// Registers a dynamic cosmetic type and its associated properties for use in the system.
    /// Also registers the ID of the cosmetic type, which is derived from the type name of the cosmetic.
    /// </summary>
    /// <remarks>This method ensures that the specified cosmetic type and its properties are registered in the
    /// system. If the cosmetic type is already registered under the given name, the method will skip registration and
    /// log a debug message.</remarks>
    /// <typeparam name="CosmeticT">The type of the dynamic cosmetic, which must inherit from <see cref="DynamicCreatureCosmetic"/>.</typeparam>
    /// <typeparam name="PropertiesT">The type of the properties associated with the cosmetic, which must inherit from <see cref="DynamicCreatureCosmetic.Properties"/>.</typeparam>
    /// <param name="name">The unique name used to identify the cosmetic type. 
    /// This name must be prepared for cosmetic reference and should not conflict with existing registered types.</param>
    public static void RegisterDynamicCosmeticType<CosmeticT, PropertiesT>() where CosmeticT : DynamicCreatureCosmetic where PropertiesT : DynamicCreatureCosmetic.Properties
    {
        Type cosmeticType = GetTypeFromGeneric<CosmeticT>();
        Type propertiesType = GetTypeFromGeneric<PropertiesT>();

        var cosmeticTypeID = ConvertCosmeticTypeToID(cosmeticType);

        if (dynamicCosmeticTypes.ContainsKey(cosmeticTypeID))
        {
            Plugin.LogDebug($"Dynamic cosmetic type id: {cosmeticTypeID} is already registered, skipping");
        }
        else
        {
            dynamicCosmeticTypes.Add(cosmeticTypeID, new DynamicCosmeticTypeInfo(cosmeticType, propertiesType));

            if (loadedDynamicCosmeticProperties.ContainsKey(cosmeticTypeID))
            {
                Plugin.LogError($"Dynamic cosmetic type id: {cosmeticTypeID} is already registered with properties somehow???, skipping registration");
            }
            else
            {
                var typeProperties = propertiesType.GetNestedType("Properties");
                loadedDynamicCosmeticProperties.Add(cosmeticTypeID, new Dictionary<string, DynamicCreatureCosmetic.Properties>());
                Plugin.LogDebug($"Registered dynamic cosmetic type id: {cosmeticTypeID} with type: {cosmeticType} and properties of type: {propertiesType}");
            }
        }
    }

    /// <summary>
    /// Gets the type from a generic type parameter.
    /// Just a helper method in case I (Marley) mess up the syntax again.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static Type GetTypeFromGeneric<T>()
    {
        return typeof(T);
    }

    public static DynamicCreatureCosmetic.Properties GetLoadedCosmeticPropertiesForID(string dynamicCosmeticTypeID, string propertiesID)
    {
        if (!loadedDynamicCosmeticProperties.ContainsKey(dynamicCosmeticTypeID))
        {
            Plugin.LogError($"Failed to get loaded properties for {dynamicCosmeticTypeID}, type not registered!");
            Plugin.LogMessage($"Keys of current property types are: {loadedDynamicCosmeticProperties.Keys.ToString()}");
            return null;
        }
        if (loadedDynamicCosmeticProperties[dynamicCosmeticTypeID].TryGetValue(propertiesID, out var properties))
        {
            Plugin.LogDebug($"Successfully got loaded properties for properties id: {propertiesID} of cosmetic type id: {dynamicCosmeticTypeID}!");
            return properties;
        }
        else
        {
            Plugin.LogError($"Failed to get loaded properties for properties id: {propertiesID} of cosmetic type id: {dynamicCosmeticTypeID}!s");
            return null;
        }
    }

    private static string GetCosmeticPropertiesIDFromPath(string path)
    {
        // Remove the file extension and return the name.
        return Path.GetFileNameWithoutExtension(path);
    }

    private static string[]? GetDirectoryForCosmeticPropertiesOfCosmeticTypeID(string cosmeticTypeID)
    {
        //-- MR7: Get the path to the cosmetic preset name, path is decided directly from the name of the type for consitency.
        var directory = AssetManager.ListDirectory(Plugin.CosmeticPropertiesDirectory + "/" + cosmeticTypeID);

        if (directory == null || directory.Length == 0)
            return null;
        else
            return directory;
    }

    private static void LoadCosmeticPropertiesOfCosmeticType(string dynamicCosmeticTypeID)
    {
        Plugin.LogDebug($"Loading cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID}");

        try
        {
            var dynamicCosmeticTypeInfo = dynamicCosmeticTypes[dynamicCosmeticTypeID];
            var directory = GetDirectoryForCosmeticPropertiesOfCosmeticTypeID(dynamicCosmeticTypeID);

            if (directory == null)
            {
                Plugin.LogError($"Failed to get directory for cosmetic properties of type id: {dynamicCosmeticTypeID}, returning");
                return;
            }

            foreach (string path in directory)
            {
                var cosmeticPropertiesID = GetCosmeticPropertiesIDFromPath(path);

                Plugin.LogDebug($"Loading cosmetic properties id: {cosmeticPropertiesID} at path: {path}");

                var jsonData = Json.Parser.Parse(File.ReadAllText(path)) as Dictionary<string, object>;
                if (jsonData == null)
                {
                    Plugin.LogError($"Failed to parse JSON data for cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID} at path: {path}, JSON Data is null");
                    continue;
                }

                var properties = Activator.CreateInstance(dynamicCosmeticTypeInfo.PropertiesType) as DynamicCreatureCosmetic.Properties;
                var parsedProperties = properties.ParseProperties(jsonData);
                if (parsedProperties == null)
                {
                    Plugin.LogError($"Failed to parse properties for cosmetic properties for cosmetic type id: {dynamicCosmeticTypeID} at path: {path}, parsedProperties is null");
                    continue;
                }

                AddLoadedDynamicCosmeticProperties(dynamicCosmeticTypeID, cosmeticPropertiesID, parsedProperties);

                Plugin.LogDebug($"Loaded property id: {cosmeticPropertiesID} your proof is that the scale var of the property is ({parsedProperties.scaleX}, {parsedProperties.scaleY})");
            }
        }
        catch (Exception e)
        {
            Plugin.LogError($"Failed to load cosmetic properties of type id: {dynamicCosmeticTypeID}, Exception: {e}");
        }
    }

    public static void LoadCosmeticProperties()
    {
        //-- MR7: Little seperators to help it stand out ore in the log.
        Plugin.LogDebug("//");
        Plugin.LogDebug("//-- Loading cosmetic properties...");
        Plugin.LogDebug("//");

        foreach (var cosmeticType in dynamicCosmeticTypes)
            LoadCosmeticPropertiesOfCosmeticType(cosmeticType.Key);

        Plugin.LogDebug("//");
        Plugin.LogDebug("//-- Finished loading cosmetic properties.");
        Plugin.LogDebug("//");
    }
}
