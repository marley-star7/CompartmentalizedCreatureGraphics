using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

//-- MR7: TODO: Come up with a better name for this struct, more indiciative of what it is.
public struct SpriteLayerGroup
{
    public int layer;

    public int firstSpriteIndex;
    public int lastSpriteIndex;

    public bool needsReorder = false;

    public SpriteLayerGroup(int layer, int firstSpriteIndex)
    {
        this.layer = layer;
        this.firstSpriteIndex = firstSpriteIndex;
        lastSpriteIndex = firstSpriteIndex;
    }

    public SpriteLayerGroup(int layer, int firstSpriteIndex, int lastSpriteIndex)
    {
        this.layer = layer;
        this.firstSpriteIndex = firstSpriteIndex;
        this.lastSpriteIndex = lastSpriteIndex;
    }
}
