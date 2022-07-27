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
using System.Collections.Generic;
using UnityEngine;
using static Moondown.Player.Movement.PlayerMovement;

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

        public static GameObject GetChild(this GameObject gameObject, string name)
        {
            foreach (Transform t in gameObject.transform)
                if (t.gameObject.name == name)
                    return t.gameObject;

            return null;
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

        public static Facing Reverse(this Facing facing) => facing == Facing.Left ? Facing.Right : Facing.Left;

        public static string Display<T>(this T[] arr, string sep=", ")
        {
            string @out = "";

            foreach (T element in arr)
            {
                @out += element.ToString() + sep;
            }

            return @out;
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

        public static int HasGravity(this Mode mode)
        {
            if (mode == Mode.Normal)
                return 1;
            else
                return 0;
        }
    }
}