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

using Moondown;
using Moondown.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AIDebugger : EditorWindow
{
    private Dictionary<string, bool> clicked = new Dictionary<string, bool>();

    [MenuItem("Window/Moonown/AI Debugger")]
    public static void ShowWindow()
    {
        GetWindow<AIDebugger>("AI Debugger");
    }

    private void OnGUI()
    {
        GUILayout.Label("Moondown AI Debugger");
        GUILayout.Space(10);

        GenControllers();
    }

    private void GenControllers()
    {
        if (GameManager.Instance == null)
            return;

        foreach (Controller controller in GameManager.Instance.Controllers)
        {
            Button(controller.Name, OnDropdownClicked);

            foreach (Unit unit in controller.Units)
            {
                if (clicked.ContainsKey(controller.Name) && clicked[controller.Name])
                    GUILayout.Label("    " + unit.name);
            }
        }
    }

    private void OnDropdownClicked(string content)
    {
        if (clicked.ContainsKey(content))
            clicked[content] = !clicked[content];
        else
            clicked.Add(content, true);
    }

    private void Button(string text, Action<string> callback)
    {
        if (GUILayout.Button(text, EditorStyles.label))
        {
            callback(text);
        }
    }

}
