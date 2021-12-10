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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset[] translations;

    private void Start() => translate();

    private void translate()
    {
        Dictionary<string, Dictionary<string, string>> locales = new Dictionary<string, Dictionary<string, string>> { };


        // get all key-value pairs
        foreach (TextAsset locale in translations)
        {
            locales.Add(locale.name, new Dictionary<string, string> { });

            foreach (string line in locale.text.Split(char.Parse("\n")))
            {
                string key = line.Split(char.Parse("="))[0];
                string value = line.Split(char.Parse("="))[1];

                locales[locale.name].Add(key, value);
            }
        }

        // get all relevant text objects
        List<Text> toTranslate = new List<Text> { };
        foreach (Transform @object in GameObject.Find("Canvas").transform)
            GetChildren(@object.gameObject, ref toTranslate);

        // get current locale
        Dictionary<string, string> currentLocale = locales["en-gb"];

        // translate
        foreach (Text text in toTranslate)
        {
            if (currentLocale.ContainsKey(text.text))
            {
                text.text = currentLocale[text.text];

                if (text.gameObject.GetComponent<DynamicText>() != null)
                    text.gameObject.GetComponent<DynamicText>().Replace(text);
            }
        }

    }

    private void GetChildren(GameObject parent, ref List<Text> toTranslate)
    {
        if (parent.GetComponent<Text>() != null)
            toTranslate.Add(parent.GetComponent<Text>());

        foreach (Transform child in parent.transform)
        {
            if (child.GetComponent<Text>() != null)
                toTranslate.Add(child.GetComponent<Text>());
            GetChildren(child.gameObject, ref toTranslate);
        }
    }

}
