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

        public static bool Has<T>(this GameObject gameObject) where T : Component => gameObject.GetComponent<T>() != null;
        public static bool Has<T>(this Component component) where T : Component => component.GetComponent<T>() != null;
        public static bool ChildHas<T>(this GameObject gameObject) where T : Component => gameObject.GetComponentInChildren<T>() != null;

        public static GameObject[] GetChildren(this GameObject gameObject)
        {
            List<GameObject> @out = new List<GameObject>();
            foreach (Transform t in gameObject.transform)
                @out.Add(t.gameObject);

            return @out.ToArray();
        }

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

        public static string CapitalizeFirst(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };
        }
    }
}