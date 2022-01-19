/*
    Copyright (C) 2021 Moondown Project

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Moondown.Player.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Utility
{
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
        public static Sprite MergeSprites(this Sprite main, params Sprite[] overlay)
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

        public static bool Has<T>(this GameObject gameObject) where T : Component => gameObject.GetComponent<T>() != null;
        public static bool Has<T>(this Component component) where T : Component => component.GetComponent<T>() != null;

        public static int ToAxis(this float @float, int previous)
        {
            if (@float > 0)
                return 1;
            else if (@float < 0)
                return -1;
            else
                return previous;
        }

        public static Facing Reverse(this Facing facing) => (Facing)((int)facing * -1);
    }
}