/*
using UnityEngine;
using RWCustom;

namespace CompartmentalizedCreatureGraphics;

public sealed class CCGConfig : OptionInterface
{
    public static CCGConfig Instance { get; } = new();

    public static Configurable<bool> VanillaSlugcatsCompartmentalized;

    public CCGConfig()
    {
        VanillaSlugcatsCompartmentalized = config.Bind(
            "requireCansGraffiti",
            true,
            new ConfigurableInfo("Requires a spray can to spray graffiti on the background (craft a can with a rock and a colorful item).",
            tags: ["Require Spray Cans for Graffiti"]
        ));
    }

    // Called when the config menu is opened by the player.
    public override void Initialize()
    {
        base.Initialize();

        // Options tab
        AddTitle(0, "Gameplay", 570f);
        AddCheckbox(RequireCansGraffiti, 540f);
    }
}*/