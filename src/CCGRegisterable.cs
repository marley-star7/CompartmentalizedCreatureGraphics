namespace CompartmentalizedCreatureGraphics;

public abstract class CCGRegisterable
{
    public abstract Type DynamicCreatureCosmeticType { get; }
    public abstract Type DynamicCreatureCosmeticPropertiesType { get; }

    public string CosmeticTypeID => CCG.ConvertTypeToCCGID(DynamicCreatureCosmeticType);

    /// <summary>
    /// String is the properties ID, which is used to reference the loaded properties.
    /// </summary>
    protected readonly Dictionary<string, CCGCosmeticProperties> loadedProperties = new();

    /// <summary>
    /// This should parse the json data sent to the proper properties, and return.
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns></returns>
    public abstract DynamicCreatureCosmetic.Properties ParseProperties(string json);

    /// <summary>
    /// Adds a set of dynamic cosmetic properties to the collection of loaded properties.
    /// </summary>
    /// <remarks>If the specified <paramref name="propertiesId"/> already exists in the collection, the
    /// addition is skipped, and a warning is LogCCGged.</remarks>
    /// <param name="propertiesId">The unique identifier for the cosmetic properties. This identifier is processed to ensure it is suitable for
    /// reference.</param>
    /// <param name="properties">The dynamic cosmetic properties to be added.</param>
    public void AddLoadedProperties(string propertiesId, DynamicCreatureCosmetic.Properties properties)
    {
        propertiesId = CosmeticManager.PrepareStringForReference(propertiesId);

        if (loadedProperties.ContainsKey(propertiesId))
        {
            Plugin.LogCCGWarning($"Dynamic cosmetic properties with ID: {propertiesId} is already loaded, skipping addition.");
            return;
        }
        if (properties == null)
        {
            Plugin.LogCCGError($"Failed adding loaded dynamicCosmeticProperties, Properties is null??");
            return;
        }

        loadedProperties.Add(propertiesId, properties);
        Plugin.LogCCGDebug($"Added loaded property id: {propertiesId} to {this.GetType().Name} your proof is that the scale var of the property is ({properties.scaleX}, {properties.scaleY})");
    }

    /// <summary>
    /// Retrieves the loaded dynamic cosmetic properties associated with the specified properties ID.
    /// </summary>
    /// <param name="propertiesId"></param>
    /// <returns></returns>
    public CCGCosmeticProperties? GetLoadedPropertiesFromPropertiesId(string propertiesId)
    {
        propertiesId = CosmeticManager.PrepareStringForReference(propertiesId);
        if (loadedProperties.TryGetValue(propertiesId, out var properties))
            return properties;

        Plugin.LogCCGError($"Could not get loaded properties for properties id: {propertiesId}, returning null");
        return null;
    }

    /// <summary>
    /// Attempts to retrieve the loaded dynamic cosmetic properties associated with the specified properties ID.
    /// </summary>
    /// <param name="propertiesId"></param>
    /// <param name="properties"></param>
    public bool TryGetLoadedPropertiesFromPropertiesId(string propertiesId, out CCGCosmeticProperties? properties)
    {
        propertiesId = CosmeticManager.PrepareStringForReference(propertiesId);
        return loadedProperties.TryGetValue(propertiesId, out properties);
    }
}
