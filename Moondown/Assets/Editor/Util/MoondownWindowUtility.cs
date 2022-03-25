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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Moondown.Utility
{
    public static class MoondownWindowUtility
    {
        public static void CallbackButton(string text, Action<string> callback)
        {
            if (GUILayout.Button(text, EditorStyles.label))
            {
                callback(text);
            }
        }

        public static void VerticalLayout(Action action)
        {
            GUILayout.BeginVertical();
            action();
            GUILayout.EndVertical();
        }

        public static void HorizontalLayout(Action action)
        {
            GUILayout.BeginHorizontal();
            action();
            GUILayout.BeginHorizontal();
        }
    }
}
