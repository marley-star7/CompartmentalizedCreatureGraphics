using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Core;

public class DynamicCosmeticPreset
{
    public DynamicCosmetic dynamicCosmetic;

    public string name;

    public DynamicCosmeticPreset(string name, DynamicCosmetic dynamicCosmetic)
    {
        this.name = name;
        this.dynamicCosmetic = dynamicCosmetic;
    }
}
