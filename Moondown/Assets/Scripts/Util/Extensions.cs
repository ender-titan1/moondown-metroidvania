using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Overlay(this Sprite sprite, Color color, bool doTransparent)
    {
        for (int x = 0; x < sprite.texture.width; x++)
        {
            for (int y = 0; y < sprite.texture.height; y++)
            {
                if (doTransparent || sprite.texture.GetPixel(x, y).a != 0)
                    sprite.texture.SetPixel(x, y, sprite.texture.GetPixel(x, y) + color);
            }
        }

        sprite.texture.Apply();
    }

    /// <summary>
    /// A Sprite array extension method that merges two or more sprites
    /// </summary>
    /// <remarks>
    /// Not very preformant, should be only used during loading assets.
    /// </remarks>
    /// <param name="sprites">The sprites that will be merged</param>
    /// <returns>The merged Sprite</returns>
    public static Sprite MergeSprites(this Sprite[] sprites)
    {
        Resources.UnloadUnusedAssets();
        Texture2D newTexture = new Texture2D(320, 320);

        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                newTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                for (int x = 0; x < newTexture.width; x++)
                {
                    Color color = sprites[i].texture.GetPixel(x, y).a == 0 ?
                        newTexture.GetPixel(x, y) :
                        sprites[i].texture.GetPixel(x, y);

                    newTexture.SetPixel(x, y, color);
                }
            }
        }

        newTexture.Apply();
        Sprite finalSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
        finalSprite.name = "Inevntory slot";
        return finalSprite;
    }
}
