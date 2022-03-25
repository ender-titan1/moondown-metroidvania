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
using Moondown.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUILayout;

public class AIDebugger : EditorWindow
{
    private Dictionary<string, bool> clicked = new Dictionary<string, bool>();

    private Unit current;

    [MenuItem("Window/Moonown/AI Debugger")]
    public static void ShowWindow()
    {
        GetWindow<AIDebugger>("AI Debugger");
    }

    private void OnGUI()
    {
        Label("Moondown AI Debugger");
        Space(10);

        MoondownWindowUtility.HorizontalLayout(
            () =>
            {
                MoondownWindowUtility.VerticalLayout(GenControllers);

                MoondownWindowUtility.VerticalLayout(GenData);
            }
        );
    }

    private void GenData()
    {
        if (current == null)
            return;

        Label("Name:", EditorStyles.boldLabel);
        Label(current.name);
        Space(5);

        Label("Controller:", EditorStyles.boldLabel);
        Label(current.Controller != null ? current.Controller.Name : "None");
        Space(5);

        Label("Facing:", EditorStyles.boldLabel);
        Label(current.Facing.ToString());
        Space(5);
    }

    private void GenControllers()
    {

        if (GameManager.Instance == null)
            return;

        foreach (Controller controller in GameManager.Instance.Controllers)
        {
            MoondownWindowUtility.CallbackButton(controller.Name, OnDropdownClicked);

            foreach (Unit unit in controller.Units)
            {
                if (clicked.ContainsKey(controller.Name) && clicked[controller.Name])
                    MoondownWindowUtility.CallbackButton(
                        "    " + unit.name, 
                        (text) => 
                        {
                            current = unit;
                            Selection.activeGameObject = unit.gameObject;
                        }
                    );
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


}
