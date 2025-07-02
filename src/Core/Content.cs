using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Core;

public static class Content
{
    /// <summary>
    /// The assigned preset for each slugcat. This is used to determine the cosmetics that are applied to each slugcat.
    /// </summary>
    public readonly static Dictionary<SlugcatStats.Name, SlugcatCosmeticsPreset> assignedSlugcatPresets = new();

    public readonly static List<DynamicCosmeticPreset> dynamicCosmeticPresets = new();

    public readonly static List<SlugcatCosmeticsPreset> characterCosmeticPresets = new();

    public readonly static List<SlugcatCosmeticsPreset> customCharacterCosmeticPresets = new();

    public static void AddDynamicCosmeticPreset(DynamicCosmeticPreset preset)
    {
        dynamicCosmeticPresets.Add(preset);
    }

    /// <summary>
    /// Adds a locked character cosmetic preset. These are presets unable to be removed as they are based off characters.
    /// </summary>
    /// <param name="preset"></param>
    public static void AddCharacterCosmeticPreset(CharacterCosmeticPreset preset)
    {
        characterCosmeticPresets.Add(preset);
    }

    /// <summary>
    /// Adds a custom character cosmetic preset. These are presets that can be added and removed by the player.
    /// Internal to make sure modders don't accidentally use this one, as it is meant for custom cosmetics that are not based on characters, but rather player-made cosmetics.
    /// </summary>
    /// <param name="preset"></param>
    internal static void AddCustomCharacterCosmeticPreset(CharacterCosmeticPreset preset)
    {
        customCharacterCosmeticPresets.Add(preset);
    }
}
