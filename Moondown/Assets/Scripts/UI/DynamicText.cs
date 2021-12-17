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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DynamicText : MonoBehaviour
{
    [SerializeField]
    private string[] inputs;

    private string template;

    private void Update() => Replace(false);

    public void Replace(bool firstTime)
    {
        Text text = gameObject.GetComponent<Text>();

        if (firstTime)
            template = text.text;

        string updatedString = template;

        for (int i = 0; i < inputs.Length; i++)
        {
            string str = updatedString.Replace("{" + i.ToString() + "}", GenInput(inputs[i]));
            text.text = str;
            updatedString = str;
        }
    }

    private string GenInput(string str)
    {

        string[] strings = str.Split(char.Parse("."));

        Type type = GetTypeByName(strings[0]);
        object instance = type.GetProperty(strings[1]).GetValue(null);

        PropertyInfo prop = type.GetProperty(strings[2]);

        if (prop.GetValue(instance) != null)
            return prop.GetValue(instance).ToString();
        else
            return "nothing";
    }

    private Type GetTypeByName(string name)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var tt = assembly.GetType(name);
            if (tt != null)
                return tt;
        }

        return null;
    }
}
