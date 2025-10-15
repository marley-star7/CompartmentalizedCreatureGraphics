namespace CompartmentalizedCreatureGraphics.Cosmetics.Slugcat;

/*
public class DynamicSlugcatTailCosmetic
{
    public static void ReplaceTailGraphics(PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, bool uvMapped)
    {
        if (!PlayerGraphicsData.TryGetValue(self, out var playerGraphicsData))
        {
            return;
        }

        if (sLeaser.sprites[2] is TriangleMesh tail)
        {
            playerGraphicsData.tailSegmentRef = self.tail;

            //-WW -ONLY RESIZE THE TAIL IF WE HAVE A CUSTOM TAIL SIZE
            if (Customization.For(self).CustomTail.IsCustom)
            {
                //sLeaser.sprites[2].RemoveFromContainer();

                Triangle[] triangles = new Triangle[((self.tail.Length - 1) * 4) + 1];
                for (int i = 0; i < self.tail.Length - 1; i++)
                {
                    int num = i * 4;
                    for (int j = 0; j < 4; j++)
                    {
                        triangles[num + j] = new Triangle(num + j, num + j + 1, num + j + 2);
                    }
                }
                triangles[(self.tail.Length - 1) * 4] = new Triangle((self.tail.Length - 1) * 4, ((self.tail.Length - 1) * 4) + 1, ((self.tail.Length - 1) * 4) + 2);

                //tail = new TriangleMesh("Futile_White", array, tail.customColor, false);
                //sLeaser.sprites[2] = tail;
                playerGraphicsData.tailRef = tail;

                //-- This is so wonky, why can't we just create a new tail? I blame SplitScreenCoop
                tail.triangles = triangles;
                int verticeNum = 2;
                for (int i = 0; i < tail.triangles.Length; i++)
                {
                    if (tail.triangles[i].a > verticeNum)
                    {
                        verticeNum = tail.triangles[i].a;
                    }
                    if (tail.triangles[i].b > verticeNum)
                    {
                        verticeNum = tail.triangles[i].b;
                    }
                    if (tail.triangles[i].c > verticeNum)
                    {
                        verticeNum = tail.triangles[i].c;
                    }
                }
                tail.vertices = new Vector2[verticeNum + 1];
                tail.UVvertices = new Vector2[verticeNum + 1];
                for (int j = 0; j < verticeNum; j++)
                {
                    tail.vertices[j] = new Vector2(0f, 0f);
                    tail.UVvertices[j] = new Vector2(0f, 0f);
                }
                if (tail.customColor)
                {
                    tail.verticeColors = new Color[verticeNum + 1];
                    for (int k = 0; k < tail.verticeColors.Length; k++)
                    {
                        tail.verticeColors[k] = tail._alphaColor;
                    }
                }
                tail.Init(FFacetType.Triangle, tail.element, triangles.Length); ;

                //rCam.ReturnFContainer("Midground").AddChild(tail);
                //tail.MoveBehindOtherNode(sLeaser.sprites[4]);
            }
            //THE REST WE ALWAYS RUN, BECAUSE WE NEED TO APPLY CUSTOM TEXTURES

            if (playerGraphicsData.SpriteReplacements.TryGetValue("TailTexture", out var tailTexture) && tailTexture != null)
            {
                tail.element = tailTexture;
            }
            else
            {
                tail.element = playerGraphicsData.originalTailElement;
            }

            if (!uvMapped)
            {
                MapTailUV(self, playerGraphicsData, tail);
            }
        }
    }

    private static void MapTailUV(PlayerGraphics self, PlayerGraphicsEx playerGraphicsData, TriangleMesh tail)
    {
        const float ASYM_SCALE_FAC = 3.0f; //-FB asymmetric tail is 3 times as wide as normal

        float uvYOffset = 0.0f;
        float scaleFac = 1.0f;

        //-FB copy pasted from pearlcat, what could go wrong?
        if (Customization.For(self).CustomTail.IsAsym)
        {
            scaleFac = ASYM_SCALE_FAC;

            Vector2 legsPos = self.legs.pos;
            Vector2 tailPos = self.tail[0].pos;

            // Find the difference between the x positions and convert it into a 0.0 - 1.0 ratio between the two
            float difference = tailPos.x - legsPos.x;


            const float minEffectiveOffset = -15.0f;
            const float maxEffectiveOffset = 15.0f;

            float leftRightRatio = Mathf.InverseLerp(minEffectiveOffset, maxEffectiveOffset, difference);


            // Multiplier determines how many times larger the texture is vertically relative to the displayed portion
            uvYOffset = Mathf.Lerp(0.0f, tail.element.uvTopRight.y - (tail.element.uvTopRight.y / scaleFac), leftRightRatio);
        }

        for (int vertex = tail.vertices.Length - 1; vertex >= 0; vertex--)
        {
            float interpolation = vertex / 2.0f / (tail.vertices.Length / 2.0f);
            Vector2 uvInterpolation;

            // Even vertexes
            if (vertex % 2 == 0)
                uvInterpolation = new Vector2(interpolation, 0.0f);

            // Last vertex
            else if (vertex == tail.vertices.Length - 1)
                uvInterpolation = new Vector2(1.0f, 0.0f);

            else
                uvInterpolation = new Vector2(interpolation, 1.0f);

            Vector2 uv;
            uv.x = Mathf.Lerp(tail.element.uvBottomLeft.x, tail.element.uvTopRight.x, uvInterpolation.x);
            uv.y = Mathf.Lerp(tail.element.uvBottomLeft.y + uvYOffset, (tail.element.uvTopRight.y / scaleFac) + uvYOffset, uvInterpolation.y);

            tail.UVvertices[vertex] = uv;

            if (tail.verticeColors != null && playerGraphicsData.originalTailColors != null)
            {
                var colorIndex = vertex % playerGraphicsData.originalTailColors.Length;
                tail.verticeColors[vertex] = playerGraphicsData.originalTailColors[colorIndex];
            }
        }
    }

}
*/