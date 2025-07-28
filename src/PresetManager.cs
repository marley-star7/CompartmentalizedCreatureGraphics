namespace CompartmentalizedCreatureGraphics;

public static class PresetManager
{
    /// <summary>
    /// The assigned default preset for each slugcat. This is used to determine the cosmetics that are applied to each slugcat.
    /// </summary>
    public readonly static Dictionary<SlugcatStats.Name, SlugcatCosmeticsPreset> defaultSlugcatCosmeticsPresets = new();

    public readonly static Dictionary<string, SlugcatCosmeticsPreset> customSlugcatCosmeticsPresets = new();

    /// <summary>
    /// Adds a locked character cosmetic preset. These are presets unable to be removed as they are based off characters.
    /// </summary>
    /// <param name="preset"></param>
    public static void StoreSlugcatCosmeticsPreset(SlugcatStats.Name slugcatName, SlugcatCosmeticsPreset preset)
    {
        defaultSlugcatCosmeticsPresets.Add(slugcatName, preset);
    }

    //
    // LOADING
    //

    /// <summary>
    /// Load a slugcat cosmetic preset by it's name.
    /// </summary>
    /// <param name="slugcatName"></param>
    public static SlugcatCosmeticsPreset LoadDefaultSlugcatCosmeticsPreset(SlugcatStats.Name slugcatName)
    {
        if (defaultSlugcatCosmeticsPresets.TryGetValue(slugcatName, out var slugcatCosmeticsPreset))
        {
            return slugcatCosmeticsPreset;
        }
        else if (defaultSlugcatCosmeticsPresets.TryGetValue(SlugcatStats.Name.White, out var missingSlugcatCosmeticsPreset))
        {
            //-- MR7: TODO: Add a more proper default missing slugcat that is more obvious something is wrong, filled with error texture.
            // Just add another SlugcatStats.Name called "MarError" or something.

            Plugin.LogError($"Failed to find slugcat cosmetics preset for slugcat: {slugcatName}, returning default preset (Survivor)");
            return missingSlugcatCosmeticsPreset;
        }
        else
        {
            Plugin.LogError($"Failed to find slugcat cosmetics preset for slugcat: {slugcatName}, and no default preset (Survivor) is available, something has gone terribly wrong... returning null");
            return null;
        }
    }

    internal static void LoadSlugcatCosmeticsPresets()
    {
        var directory = GetDirectoryForSlugcatPresets();

        try
        {
            foreach (string path in directory)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                Plugin.LogDebug($"Loading slugcat cosmetics preset of filename: {fileName} at path: {path}");

                var jsonData = Json.Parser.Parse(File.ReadAllText(path)) as Dictionary<string, object>;
                if (jsonData == null)
                {
                    Plugin.LogError($"Failed to parse JSON data for slugcat cosmetics preset at path: {path}, JSON Data is null, skipping this preset");
                    continue;
                }

                if (!jsonData.Keys.First().TryGetExtEnum(out SlugcatStats.Name slugcatName))
                {
                    Plugin.LogError($"Failed to find slugcat name in JSON data for slugcat cosmetics preset at path: {path}, skipping this preset");
                    continue;
                }

                var slugcatPresetJsonData = jsonData.Values.First() as Dictionary<string, object>;
                if (slugcatPresetJsonData == null)
                {
                    Plugin.LogError($"Failed to parse slugcat cosmetics preset JSON data at path: {path}, JSON Data is null, skipping this preset");
                    continue;
                }

                var slugcatPreset = ParseSlugcatCosmeticsPresetJson(slugcatPresetJsonData);
                StoreSlugcatCosmeticsPreset(slugcatName, slugcatPreset);

                Plugin.LogDebug($"Loaded slugcat cosmetics preset: {fileName} for slugcat: {slugcatName}, your proof is that the base head sprite name is {slugcatPreset.baseHeadSpriteName}");
            }
        }
        catch (Exception e)
        {
            Plugin.LogError($"Failed to load slugcat cosmetics presets at path: {directory}. Exception: {e}");
        }
    }

    //
    // DIRECTORYS
    //

    private static string[] GetDirectoryForSlugcatPresets()
    {
        var directory = AssetManager.ListDirectory(Plugin.SlugcatCosmeticsPresetsDirectory);
        if (directory == null || directory.Length == 0)
        {
            Plugin.LogDebug($"No slugcat cosmetics presets found at: {Plugin.SlugcatCosmeticsPresetsDirectory}, returning");
            return null;
        }
        else
            return directory;
    }

    //
    // PARSING
    //

    private static SlugcatCosmeticsPreset ParseSlugcatCosmeticsPresetJson(Dictionary<string, object> jsonData)
    {
        var preset = new SlugcatCosmeticsPreset();

        // Parse base sprites
        ParseBaseSprites(preset, jsonData);

        // Parse dynamic cosmetics
        ParseDynamicCosmetics(preset, jsonData);

        return preset;
    }

    private static void ParseBaseSprites(SlugcatCosmeticsPreset preset, Dictionary<string, object> jsonData)
    {
        preset.baseHeadSpriteName = ParseSprite(jsonData, nameof(preset.baseHeadSpriteName), preset.baseHeadSpriteName);
        preset.baseFaceSpriteName = ParseSprite(jsonData, nameof(preset.baseFaceSpriteName), preset.baseFaceSpriteName);
        preset.baseBodySpriteName = ParseSprite(jsonData, nameof(preset.baseBodySpriteName), preset.baseBodySpriteName);
        preset.baseArmSpriteName = ParseSprite(jsonData, nameof(preset.baseArmSpriteName), preset.baseArmSpriteName);
        preset.baseLegsSpriteName = ParseSprite(jsonData, nameof(preset.baseLegsSpriteName), preset.baseLegsSpriteName);
        preset.baseHipsSpriteName = ParseSprite(jsonData, nameof(preset.baseHipsSpriteName), preset.baseHipsSpriteName);
        preset.baseTailSpriteName = ParseSprite(jsonData, nameof(preset.baseTailSpriteName), preset.baseTailSpriteName);
        preset.basePixelSpriteName = ParseSprite(jsonData, nameof(preset.basePixelSpriteName), preset.basePixelSpriteName);
    }

    private static string ParseSprite(Dictionary<string, object> jsonData, string propertyName, string defaultValue)
    {
        if (MRJson.TryParseStringProperty(jsonData, propertyName, out var spriteName))
            return spriteName;
        else
        {
            Plugin.LogError($"Failed to parse {propertyName} for slugcat cosmetics preset, using default value: {defaultValue}");
            return defaultValue;
        }
    }

    private static void ParseDynamicCosmetics(SlugcatCosmeticsPreset preset, Dictionary<string, object> jsonData)
    {
        if (!jsonData.TryGetValue("dynamicCosmetics", out var dynamicCosmeticsObj))
        {
            Plugin.LogError("Failed to find dynamicCosmetics in slugcat cosmetics preset JSON data, using empty list");
            return;
        }

        if (dynamicCosmeticsObj is not List<object> dynamicCosmeticsList)
        {
            Plugin.LogError("dynamicCosmetics is not an array, skipping dynamic cosmetics");
            return;
        }

        foreach (var item in dynamicCosmeticsList)
        {
            if (item is not List<object> cosmeticEntry || cosmeticEntry.Count < 2)
            {
                Plugin.LogError("Invalid dynamic cosmetic entry format, expected [type, name], skipping cosmetic");
                continue;
            }

            if (cosmeticEntry[0] is not string cosmeticTypeID)
            {
                Plugin.LogError("Dynamic cosmetic type name is not a string, skipping cosmetic");
                continue;
            }

            if (cosmeticEntry[1] is not string propertiesID)
            {
                Plugin.LogError("Dynamic cosmetic preset name is not a string, skipping cosmetic");
                continue;
            }

            preset.dynamicCosmetics.Add((cosmeticTypeID, propertiesID));
        }
    }
}
