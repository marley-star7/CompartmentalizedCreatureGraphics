using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics.Cosmetics;

/// <summary>
/// Group to categorizes sprites for a cosmetic by their index.
/// </summary>
public struct SpriteEffectGroup
{
    /// <summary>
    /// The indexes associated with sprites in this effect group.
    /// </summary>
    public int[] sprites = new int[0];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprites"></param>
    public SpriteEffectGroup(params int[] sprites)
    {
        if (sprites.Length != 0)
        {
            this.sprites = sprites;
        }
    }
}
