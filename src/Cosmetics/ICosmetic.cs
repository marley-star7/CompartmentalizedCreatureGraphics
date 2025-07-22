using CompartmentalizedCreatureGraphics.Cosmetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompartmentalizedCreatureGraphics;

public interface ICosmetic
{
    public RoomCamera.SpriteLeaser SpriteLeaser{ get; }

    public Dictionary<int, SpriteLayer> SpriteLayers { get; set; }

    public void Equip(Creature wearer);

    public void OnWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos);

    //-- MR7: Since RoomPalette is a struct, it's slightly more performant to use "in" keyword.
    public  void OnWearerApplyPalette(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, in RoomPalette palette);

    public void OnWearerCollide(Player player, PhysicalObject otherObject, int myChunk, int otherChunk);

    public void OnWearerTerrainImpact(Player player, int chunk, IntVector2 direction, float speed, bool firstContact);
}