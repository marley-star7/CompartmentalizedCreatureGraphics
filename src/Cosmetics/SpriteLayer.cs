using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

//-- MR7: TODO: Come up with a better name for this struct, more indiciative of what it is.
// Rename it to something like SpriteCollection, or SpriteList, it's not meant for layers anymore just to hold the start and end of a sprite list.
public struct SpriteLayer
{
    public bool needsReorder = false;

    public int firstSpriteIndex;
    public int lastSpriteIndex;

    public SpriteLayer(int firstSpriteIndex)
    {
        this.firstSpriteIndex = firstSpriteIndex;
        this.lastSpriteIndex = firstSpriteIndex;
    }

    public SpriteLayer(int firstSpriteIndex, int lastSpriteIndex)
    {
        this.firstSpriteIndex = firstSpriteIndex;
        this.lastSpriteIndex = lastSpriteIndex;
    }
}
