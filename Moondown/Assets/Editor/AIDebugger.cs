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


    private void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        Label("Moondown AI Debugger");
        Space(10);

        MoondownWindowUtility.HorizontalLayout(
            () =>
            {
                MoondownWindowUtility.VerticalLayout(GenControllers);
                MoondownWindowUtility.VerticalSpace();

                MoondownWindowUtility.VerticalLayout(GenData);
                MoondownWindowUtility.VerticalSpace();

                MoondownWindowUtility.VerticalLayout(GenControllerData);
            }
        );
    }

    private void GenControllerData()
    {
        if (current == null || current.Controller == null)
            return;

        Label("Controller Data:", EditorStyles.boldLabel);
        Space(10);

        MoondownWindowUtility.TextBlock("State:", current.Controller.Searching ? "Searching" : "Engaged");
    }

    private void GenData()
    {
        if (current == null || current.State == null)
            return;

        Label("Unit Data:", EditorStyles.boldLabel);
        Space(10);

        MoondownWindowUtility.TextBlock("Name:", current.name);
        MoondownWindowUtility.TextBlock("Controller:", current.Controller != null ? current.Controller.Name : "None");
        MoondownWindowUtility.TextBlock("Facing:", current.Facing.ToString(), 8);
        MoondownWindowUtility.TextBlock("State:", current.State.GetType().Name);
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
