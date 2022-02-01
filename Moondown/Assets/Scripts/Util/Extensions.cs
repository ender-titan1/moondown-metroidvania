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
using Moondown.Inventory;
using Moondown.Player.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Utility
{
    public static class Extensions
    {

        /// <summary>
        /// A Sprite array extension method that merges two or more sprites
        /// </summary>
        /// <remarks>
        /// Not very preformant, should be only used during loading assets.
        /// </remarks>
        /// <param name="main">The base sprite</param>
        /// <param name="overlay">The sprites to be merged</param>
        /// <returns>The merged Sprite</returns>
        public static Sprite MergeSprites(this Sprite main, params Sprite[] overlay)
        {
            Sprite[] sprites = new Sprite[overlay.Length + 1];
            sprites[0] = main;

            for (int i = 0; i < overlay.Length; i++)
            {
                sprites[i + 1] = overlay[i];
            }

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

        public static string Display<T>(this T[] arr)
        {
            string @out = "";

            foreach (T element in arr)
            {
                @out += element.ToString() + ", ";
            }

            return @out;
        }

        public static List<ItemStack> MakeStacks(this List<Item> items)
        {
            HashSet<Item> set = new HashSet<Item>();
            List<string> names = new List<string>();

            foreach (Item item in items)
            {
                string name = item.data.nameKey;
                if (!names.Contains(name))
                {
                    names.Add(name);
                    set.Add(item);
                }
            }


            Dictionary<Item, int> amounts = new Dictionary<Item, int>();

            foreach (Item i in set)
            {
                int count = 0;

                foreach (Item item in items)
                {
                    if (item.data.nameKey == i.data.nameKey)
                        count++;
                }

                amounts[i] = count;
            }


            List<ItemStack> stacks = new List<ItemStack>();

            foreach (Item item in amounts.Keys)
            {
                int amount = amounts[item];

                while (amount >= item.data.stackSize)
                {
                    amount -= item.data.stackSize;
                    stacks.Add(new ItemStack(item, item.data.stackSize));
                }

                if (amount != 0)
                    stacks.Add(new ItemStack(item, amount));
            }

            return stacks;
        }
    }
}