using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics;

public class SlugcatCosmeticsPreset
{
    List<DynamicCosmetic> dynamicCosmetics = new List<DynamicCosmetic>();

    public string name;

    public string baseHeadSpriteName;
    public string baseFaceSpriteName;
    public string baseBodySpriteName;
    public string baseArmSpriteName;
    public string baseLegsSpriteName;
    public string baseHipsSpriteName;
    public string baseTailSpriteName;
    public string basePixelSpriteName;

    public SlugcatCosmeticsPreset(string name, params DynamicCosmetic[] dynamicCosmetics)
    {
        this.name = name;
        this.dynamicCosmetics.AddRange(dynamicCosmetics);
    }
}
