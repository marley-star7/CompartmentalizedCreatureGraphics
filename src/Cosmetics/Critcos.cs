namespace CompartmentalizedCreatureGraphics.Cosmetics;

public abstract class Critcos
{
    private readonly static Dictionary<string, Dictionary<string, DynamicCreatureCosmetic.Properties>> loadedDynamicCosmeticProperties = new();

    public Critcos()
    {
    }

    public abstract DynamicCreatureCosmetic.Properties ParseProperties(Dictionary<string, object> jsonData);

    // TODO: move the dictionary containing loaded properties to this class, so that it can be used to retrieve properties by ID.
    public abstract DynamicCreatureCosmetic.Properties GetPropertiesFromPropertiesID(string propertiesID);

    //
    // Utility methods
    //

    public void AddLoadedDynamicCosmeticProperties(string cosmeticTypeID, string propertiesID, DynamicCreatureCosmetic.Properties properties)
    {
        cosmeticTypeID = CosmeticManager.PrepareStringForReference(cosmeticTypeID);
        propertiesID = CosmeticManager.PrepareStringForReference(cosmeticTypeID);

        if (!loadedDynamicCosmeticProperties.TryGetValue(cosmeticTypeID, out var propertiesDictionary))
        {
            Plugin.LogDebug($"No loaded dynamic cosmetic properties found for type ID: {cosmeticTypeID}");
            return;
        }

        propertiesDictionary.Add(propertiesID, properties);
    }

    public DynamicCreatureCosmetic.Properties? GetLoadedDynamicCosmeticProperties(string cosmeticTypeID, string propertiesID)
    {
        cosmeticTypeID = CosmeticManager.PrepareStringForReference(cosmeticTypeID);
        propertiesID = CosmeticManager.PrepareStringForReference(cosmeticTypeID);

        if (!loadedDynamicCosmeticProperties.TryGetValue(cosmeticTypeID, out var propertiesDictionary))
        {
            Plugin.LogDebug($"No loaded dynamic cosmetic properties found for type ID: {cosmeticTypeID}");
            return null;
        }
        if (!propertiesDictionary.TryGetValue(propertiesID, out var properties))
        {
            Plugin.LogDebug($"No loaded dynamic cosmetic properties found for type ID: {cosmeticTypeID} and properties ID: {propertiesID}");
            return null;
        }
        return properties;
    }
}