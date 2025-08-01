using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

//-- MR7: TODO: Come up with a better name for this struct, more indiciative of what it is.
public struct SpriteLayerGroup
{
    [JsonProperty("layer")]
    public int layer;

    [JsonProperty("startSpriteIndex")]
    public int startSpriteIndex;

    [JsonProperty("endSpriteIndex")]
    public int endSpriteIndex;

    public bool needsReorder = false;

    public SpriteLayerGroup(int layer, int firstSpriteIndex)
    {
        this.layer = layer;
        this.startSpriteIndex = firstSpriteIndex;
        endSpriteIndex = firstSpriteIndex;
    }

    public SpriteLayerGroup(int layer, int firstSpriteIndex, int lastSpriteIndex)
    {
        this.layer = layer;
        this.startSpriteIndex = firstSpriteIndex;
        this.endSpriteIndex = lastSpriteIndex;
    }
}
