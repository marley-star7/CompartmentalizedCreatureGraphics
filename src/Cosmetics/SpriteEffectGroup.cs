using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

public struct SpriteEffectGroup
{
    public int[] sprites;

    public SpriteEffectGroup(params int[] sprites)
    {
        if (sprites.Length > 0)
            this.sprites = sprites;
        else
            this.sprites = new int[0];
    }
}
