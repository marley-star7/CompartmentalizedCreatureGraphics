namespace CompartmentalizedCreatureGraphics;

// There are two types of dependencies:
// 1. BepInDependency.DependencyFlags.HardDependency - The other mod *MUST* be installed, and your mod cannot run without it. This ensures their mod loads before yours, preventing errors.
// 2. BepInDependency.DependencyFlags.SoftDependency - The other mod doesn't need to be installed, but if it is, it should load before yours.
//[BepInDependency("author.some_other_mods_guid", BepInDependency.DependencyFlags.HardDependency)]

[BepInDependency("marley-star7.marcustom", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("slime-cubed.slugbase", BepInDependency.DependencyFlags.SoftDependency)]

[BepInPlugin(ID, NAME, VERSION)]
sealed class Plugin : BaseUnityPlugin
{
    public const string ID = "marley-star7.ccg"; //-- This should be the same as the id in modinfo.json!
    public const string NAME = "Compartmentalized Creature Graphics"; //-- This should be a human-readable version of your mod's name. This is used for log files and also displaying which mods get loaded. In general, it's a good idea to match this with your modinfo.json as well.
    public const string VERSION = "0.0.1"; //-- This follows semantic versioning. For more information, see https://semver.org/ - again, match what you have in modinfo.json

    /// <summary>
    /// This is the directory where the default cosmetics are stored. It is used to load the default cosmetics from json files.
    /// </summary>
    public const string CosmeticPropertiesDirectory = "ccg/cosmetics";
    /// <summary>
    /// This is the directory where the sprite angle properties are stored. It is used to load the sprite angle properties from json files.
    /// </summary>
    public const string SpriteAnglePropertiesDirectory = "ccg/spriteangles";
    /// <summary>
    /// This is the directory where the default character presets are stored. It is used to load the default character presets from json files.
    /// </summary>
    public const string SlugcatCosmeticsPresetsDirectory = "ccg/slugcats";

    /// <summary>
    /// This is the directory where the custom cosmetics are stored. It is used to load the custom cosmetics from json files.
    /// </summary>
    public const string CustomCosmeticPropertiesDirectory = "designmyslugcat/cosmetics";
    /// <summary>
    /// This is the directory where the custom cosmetics are stored. It is used to load the custom cosmetics from json files.
    /// </summary>
    public const string CustomSlugcatCosmeticsPresetsDirectory = "designmyslugcat/slugcats";

    public static ProcessManager.ProcessID DesignMenu => new("DesignMenu", register: true);
    public static SlugcatStats.Name designSlugcat = new SlugcatStats.Name("DesignSlugcat", register: true);

    public static bool isPostInit;
    public static bool restartMode = false;

    public static bool improvedInputEnabled;
    public static int improvedInputVersion = 0;

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private static new ManualLogSource Logger;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Plugin()
    {
        Logger = base.Logger;
    }

    public void OnEnable()
    {
        On.RainWorld.OnModsInit += Extras.WrapInit(LoadPlugin);
        On.RainWorld.PostModsInit += RainWorld_PostModsInit;

        CosmeticManager.RegisterCritcos(new DynamicSlugcatCosmeticFaceCritcos());
        CosmeticManager.RegisterCritcos(new DynamicSlugcatCosmeticEyeCritcos());
        CosmeticManager.RegisterCritcos(new DynamicSlugcatCosmeticEarCritcos());

        Logger.LogInfo("Compartmentalized Creature Graphics is loaded!");
    }

    private static void LoadPlugin(RainWorld rainWorld)
    {
        Resources.LoadResources();

        //-- Do not re-apply hooks on restart mode!
        if (!restartMode)
        {
            Hooks.ApplyHooks();
        }
    }

    public void OnDisable()
    {
        if (restartMode)
        {
            Hooks.RemoveHooks();
        }
    }

    internal static void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld rainWorld)
    {
        orig(rainWorld);

        try
        {
            if (Plugin.isPostInit)
                return;
            else
                Plugin.isPostInit = true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e.Message);
        }

        CosmeticManager.LoadSpriteAngleProperties();
        CosmeticManager.LoadCosmeticProperties();
        PresetManager.LoadSlugcatCosmeticsPresets();
        Plugin.LogInfo("//");
        Plugin.LogInfo("//-- CCG presets and properties finished loading...");
        Plugin.LogInfo("//");
    }

    internal static void LogInfo(object ex) => Logger.LogInfo(ex);

    internal static void LogMessage(object ex) => Logger.LogMessage(ex);

    // -- Ms7: String prints are expensive!
    // So just incase we forget any #if's anywhere to encase debug logs to be for debug builds only to reduce hit on user performance.
    internal static void LogDebug(object ex)
    {
#if DEBUG
        Logger.LogDebug(ex);
#endif
    }

    internal static void LogWarning(object ex) => Logger.LogWarning(ex);

    internal static void LogError(object ex) => Logger.LogError(ex);

    internal static void LogFatal(object ex) => Logger.LogFatal(ex);
}