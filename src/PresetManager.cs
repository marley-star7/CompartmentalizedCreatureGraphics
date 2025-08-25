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
    public static SlugcatCosmeticsPreset GetDefaultSlugcatCosmeticsPreset(SlugcatStats.Name slugcatName)
    {
        if (defaultSlugcatCosmeticsPresets.TryGetValue(slugcatName, out var slugcatCosmeticsPreset))
        {
            Plugin.LogDebug($"Got slugcat cosmetic preset for slugcatStats.Name: {slugcatName}!");
            return slugcatCosmeticsPreset;
        }
        else if (defaultSlugcatCosmeticsPresets.TryGetValue(SlugcatStats.Name.White, out var missingSlugcatCosmeticsPreset))
        {
            //-- MS7: TODO: Add a more proper default missing slugcat that is more obvious something is wrong, filled with error texture.
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
        Plugin.LogInfo("//");
        Plugin.LogInfo("//-- Loading CCG slugcat cosmetics presets...");
        Plugin.LogInfo("//");

        var directory = GetDirectoryForSlugcatPresets();

        try
        {
            foreach (string path in directory)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                Plugin.LogDebug($"Loading slugcat cosmetics preset of filename: {fileName}, at path: {path}");

                var json = File.ReadAllText(path);
                var slugcatPreset = ParseSlugcatCosmeticsPresetJson(File.ReadAllText(path));

                if (slugcatPreset.name.TryGetExtEnum(out SlugcatStats.Name slugcatName))
                {
                    StoreSlugcatCosmeticsPreset(slugcatName, slugcatPreset);
                    Plugin.LogInfo($"Loaded slugcat cosmetics preset: {fileName} for slugcat: {slugcatName}, your proof is that the base head sprite name is {slugcatPreset.baseHeadSpriteName}");
                }
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

    private static SlugcatCosmeticsPreset ParseSlugcatCosmeticsPresetJson(string json)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            MissingMemberHandling = MissingMemberHandling.Error,
            Converters = { new StringTupleObjectConverter("cosmeticTypeId", "propertiesId") }
        };

        SlugcatCosmeticsPreset preset = JsonConvert.DeserializeObject<SlugcatCosmeticsPreset>(json, settings);

        return preset;
    }
}
