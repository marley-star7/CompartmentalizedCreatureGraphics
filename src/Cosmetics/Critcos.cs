namespace CompartmentalizedCreatureGraphics.Cosmetics;

public abstract class Critcos
{
    /// <summary>
    /// String is the properties ID, which is used to reference the loaded properties.
    /// </summary>
    private readonly Dictionary<string, DynamicCreatureCosmetic.Properties> loadedDynamicCosmeticProperties = new();

    public abstract Type DynamicCreatureCosmeticType { get; }
    public abstract Type DynamicCreatureCosmeticPropertiesType { get; }

    public string CosmeticTypeID => ConvertCosmeticTypeToID(DynamicCreatureCosmeticType);

    /// <summary>
    /// This should parse the json data sent to the proper properties, and return.
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns></returns>
    public abstract DynamicCreatureCosmetic.Properties ParseProperties(string json);

    /// <summary>
    /// This should return a dynamic cosmetic with loaded properties from the id, of the correct type.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="propertiesID"></param>
    /// <returns></returns>
    public abstract DynamicCreatureCosmetic CreateDynamicCosmeticForCreature(GraphicsModule graphicsModule, string propertiesId);

    //
    // Utility methods
    //

    /// <summary>
    /// Normalize the name to lowercase for consistency, so user's don't have to worry about case sensitivity when referencing cosmetics.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private string ConvertCosmeticTypeToID(Type cosmeticType)
    {
        return cosmeticType.Name.ToLowerInvariant();
    }

    /// <summary>
    /// Adds a set of dynamic cosmetic properties to the collection of loaded properties.
    /// </summary>
    /// <remarks>If the specified <paramref name="propertiesId"/> already exists in the collection, the
    /// addition is skipped, and a warning is logged.</remarks>
    /// <param name="propertiesId">The unique identifier for the cosmetic properties. This identifier is processed to ensure it is suitable for
    /// reference.</param>
    /// <param name="properties">The dynamic cosmetic properties to be added.</param>
    public void AddLoadedDynamicCosmeticProperties(string propertiesId, DynamicCreatureCosmetic.Properties properties)
    {
        propertiesId = CosmeticManager.PrepareStringForReference(propertiesId);

        if (loadedDynamicCosmeticProperties.ContainsKey(propertiesId))
        {
            Plugin.LogWarning($"Dynamic cosmetic properties with ID: {propertiesId} is already loaded, skipping addition.");
            return;
        }
        if (properties == null)
        {
            Plugin.LogError($"Failed adding loaded dynamicCosmeticProperties, Properties is null??");
            return;
        }

        loadedDynamicCosmeticProperties.Add(propertiesId, properties);
        Plugin.LogDebug($"Added loaded property id: {propertiesId} to {this.GetType().Name} your proof is that the scale var of the property is ({properties.scaleX}, {properties.scaleY})");
    }

    /// <summary>
    /// Retrieves the loaded dynamic cosmetic properties associated with the specified properties ID.
    /// </summary>
    /// <param name="propertiesId"></param>
    /// <returns></returns>
    public DynamicCreatureCosmetic.Properties? GetLoadedPropertiesFromPropertiesId(string propertiesId)
    {
        propertiesId = CosmeticManager.PrepareStringForReference(propertiesId);
        if (loadedDynamicCosmeticProperties.TryGetValue(propertiesId, out var properties))
            return properties;

        Plugin.LogError($"Could not get loaded properties for properties id: {propertiesId}, returning null");
        return null;
    }

    /// <summary>
    /// Attempts to retrieve the loaded dynamic cosmetic properties associated with the specified properties ID.
    /// </summary>
    /// <param name="propertiesId"></param>
    /// <param name="properties"></param>
    public bool TryGetLoadedPropertiesFromPropertiesId(string propertiesId, out DynamicCreatureCosmetic.Properties? properties)
    {
        propertiesId = CosmeticManager.PrepareStringForReference(propertiesId);
        return loadedDynamicCosmeticProperties.TryGetValue(propertiesId, out properties);
    }
}