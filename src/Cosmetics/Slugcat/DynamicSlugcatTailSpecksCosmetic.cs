using MoreSlugcats;

namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

public class DynamicSlugcatTailSpecksCosmetic : DynamicSlugcatTailCirclesCosmetic
{
    public new class Properties : DynamicSlugcatTailCirclesCosmetic.Properties
    {
        public override DynamicCreatureCosmetic.Properties Parse(string json)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            };

            return JsonConvert.DeserializeObject<Properties>(json, settings);
        }
    }

    public Properties properties => (Properties)_properties;

    public int NumberOfSprites => properties.rows * properties.lines + 1;

    public float spearProg = 0f;

    public int spearLine;

    public int spearRow;

    public int spearType;

    public DynamicSlugcatTailSpecksCosmetic(PlayerGraphics playerGraphics, Properties properties) : base(playerGraphics, properties)
    {
    }

    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        base.InitiateSprites(sLeaser, rCam);

        sLeaser.sprites = new FSprite[NumberOfSprites];

        for (int i = 0; i < properties.rows; i++)
        {
            for (int j = 0; j < properties.lines; j++)
            {
                if (properties.bigCircles)
                {
                    sLeaser.sprites[i * properties.lines + j] = new FSprite("Circle20", true);
                }
                else
                {
                    sLeaser.sprites[i * properties.lines + j] = new FSprite("tinyStar", true);
                }

                sLeaser.sprites[i * properties.lines + j].scaleX = properties.scaleX;
                sLeaser.sprites[i * properties.lines + j].scaleY = properties.scaleY;
            }
        }
        sLeaser.sprites[properties.rows * properties.lines] = new FSprite("BioSpear" + (this.spearType % 3 + 1).ToString(), true);
        sLeaser.sprites[properties.rows * properties.lines].anchorY = 0f;
    }

    public override void PostWearerDrawSprites(RoomCamera.SpriteLeaser wearerSLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        if (SLeaser == null)
        {
            return;
        }

        var playerGraphics = (PlayerGraphics)player.graphicsModule;

        for (int i = 0; i < properties.rows; i++)
        {
            var theVariableFuckingVariable = 0.4f;

            float f = Mathf.InverseLerp(0f, (float)(properties.rows - 1), (float)i);
            float s = Mathf.Lerp(theVariableFuckingVariable, 0.95f, Mathf.Pow(f, 0.8f));

            PlayerGraphics.PlayerSpineData playerSpineData = playerGraphics.SpinePosition(s, timeStacker);

            Color slugcatColor = playerGraphics.GetPlayerGraphicsCCGData().BaseTailSprite.color; //playerGraphics.player.SlugCatClass == MoreSlugcatsEnums.SlugcatStatsName.Slugpup) ? playerGraphics.player.ShortCutColor() : PlayerGraphics.SlugcatColor(playerGraphics.CharacterForColor);

            float num = 0.8f * Mathf.Pow(f, 0.5f);
            float num2 = (0.8f - num) * this.spearProg;

            Color speckColor = Color.Lerp(slugcatColor, Color.Lerp(properties.color, slugcatColor, 0.3f), 0.2f + num + num2);

            for (int j = 0; j < properties.lines; j++)
            {
                float speckPosFactor = GetCirclePosFactor(i, j);

                Vector2 vector = playerSpineData.pos + playerSpineData.perp * (playerSpineData.rad + 0.5f) * speckPosFactor;

                SLeaser.sprites[i * properties.lines + j].x = vector.x - camPos.x;
                SLeaser.sprites[i * properties.lines + j].y = vector.y - camPos.y;
                SLeaser.sprites[i * properties.lines + j].color = new Color(1f, 0f, 0f);
                SLeaser.sprites[i * properties.lines + j].rotation = Custom.VecToDeg(playerSpineData.dir);
                SLeaser.sprites[i * properties.lines + j].scaleX = Custom.LerpMap(Mathf.Abs(speckPosFactor), 0.4f, 1f, 1f, 0f);
                SLeaser.sprites[i * properties.lines + j].scaleY = properties.scaleY;

                if (this.spearProg > 0f)
                {
                    if (i == this.spearRow && j == this.spearLine)
                    {
                        SLeaser.sprites[i * properties.lines + j].scaleX *= 1f + this.spearProg * 2f;
                        SLeaser.sprites[i * properties.lines + j].scaleY *= 1f + this.spearProg * 2f;
                    }
                    else if ((i == this.spearRow + 1 && j == this.spearLine) || (i == this.spearRow - 1 && j == this.spearLine) || (i == this.spearRow && j == this.spearLine + 1) || (i == this.spearRow && j == this.spearLine - 1))
                    {
                        SLeaser.sprites[i * properties.lines + j].scaleX *= 1f + this.spearProg;
                        SLeaser.sprites[i * properties.lines + j].scaleY *= 1f + this.spearProg;
                    }
                }
                /*
                if (ModManager.CoopAvailable && playerGraphics.useJollyColor)
                {
                    SLeaser.sprites[i * properties.lines + j].color = PlayerGraphics.JollyColor(playerGraphics.player.playerState.playerNumber, 2);
                }
                else if (PlayerGraphics.CustomColorsEnabled())
                {
                    SLeaser.sprites[i * properties.lines + j].color = PlayerGraphics.CustomColorSafety(2);
                }
                else if (playerGraphics.CharacterForColor == SlugcatStats.Name.White || playerGraphics.CharacterForColor == SlugcatStats.Name.Yellow)
                {
                    SLeaser.sprites[i * properties.lines + j].color = Color.gray;
                }
                else
                {*/
                SLeaser.sprites[i * properties.lines + j].color = speckColor;
                //}

                if (i == this.spearRow && j == this.spearLine)
                {
                    SLeaser.sprites[properties.lines * properties.rows].x = vector.x - camPos.x;
                    SLeaser.sprites[properties.lines * properties.rows].y = vector.y - camPos.y;
                    if (ModManager.CoopAvailable && playerGraphics.useJollyColor)
                    {
                        SLeaser.sprites[properties.lines * properties.rows].color = PlayerGraphics.JollyColor(playerGraphics.player.playerState.playerNumber, 2);
                    }
                    else if (PlayerGraphics.CustomColorsEnabled())
                    {
                        SLeaser.sprites[properties.lines * properties.rows].color = PlayerGraphics.CustomColorSafety(2);
                    }
                    else
                    {
                        SLeaser.sprites[properties.lines * properties.rows].color = Color.white;
                    }
                    Vector2 v = Custom.PerpendicularVector(playerSpineData.dir);
                    if (v.normalized.y > 0.35f)
                    {
                        v.y *= -1f;
                        v.x *= -1f;
                    }

                    float rotation = Custom.VecToDeg(v);
                    SLeaser.sprites[properties.lines * properties.rows].rotation = rotation;
                    SLeaser.sprites[properties.lines * properties.rows].scaleY = -this.spearProg * 0.5f;
                    SLeaser.sprites[properties.lines * properties.rows].element = Futile.atlasManager.GetElementWithName("BioSpear" + (this.spearType % 3 + 1).ToString());
                }
            }
        }
    }

    /*
    public void newSpearSlot()
    {
        this.spearLine = UnityEngine.Random.Range(0, this.lines - 1);
        this.spearRow = UnityEngine.Random.Range(0, this.rows - 1);
    }

    public void setSpearProgress(float p)
    {
        this.spearProg = Mathf.Clamp(p, 0f, 1f);
        if (this.spearProg == 0f)
        {
            this.spearType = UnityEngine.Random.Range(0, 3);
        }
    }
    */
}
