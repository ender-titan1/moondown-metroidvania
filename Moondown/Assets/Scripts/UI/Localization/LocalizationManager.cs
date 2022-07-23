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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using TMPro;
using Moondown.Inventory;

namespace Moondown.UI.Localization
{ 

    public class LocalizationManager : MonoBehaviour
    {
        private static TextAsset[] translations;

        [SerializeField]
        private TextAsset[] visibleTranslations;

        public static string CurrentLocale { get; set; } = "en_gb";


#if UNITY_EDITOR
        [MenuItem("Moondown Tools/Refresh Translations")]
        public static void RefreshTranslations()
        {

            TextMeshProUGUI[] toCheck = GetText().ToArray();


            Dictionary<string, Dictionary<string, string>> locales = GetLocales();


            foreach (Dictionary<string, string> locale in locales.Values)
            {
                string name = locales.FirstOrDefault(x => x.Value == locale).Key;
                string path = Application.dataPath + @"/Translation/" + name + @".txt";


                foreach (TextMeshProUGUI text in toCheck)
                {
                    if (!locale.ContainsKey(text.text))
                    {

                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine("\n# To translate;");
                            sw.WriteLine(text.text + " = Temporary translation;");
                        }
                    }
                }


            }


        }
#endif

        private void Awake() => translations = visibleTranslations;

        private void Start() => Translate();

        public void ChangeLocale(string name)
        {
            CurrentLocale = name;
            Translate();

            foreach (Item item in InventoryManager.Instance.All)
            {
                item.RefreshName();
            }
        }


        private void Translate()
        {
            Dictionary<string, Dictionary<string, string>> locales = GetLocales();

            // get all relevant text objects
            List<TextMeshProUGUI> toTranslate = GetText();

            // get current locale

            Dictionary<string, string> currentLocale = new Dictionary<string, string>();

            if (locales.ContainsKey(CurrentLocale))
                currentLocale = locales[CurrentLocale];
            else
                currentLocale = locales["en_bg"];


            // translate
            foreach (TextMeshProUGUI text in toTranslate)
            {

                if (currentLocale.ContainsKey(text.text))
                {
                    text.text = currentLocale[text.text];
                }
            }

        }

        private static Dictionary<string, Dictionary<string, string>> GetLocales()
        {
            Dictionary<string, Dictionary<string, string>> locales = new Dictionary<string, Dictionary<string, string>> { };

            // get all key-value pairs
            foreach (TextAsset locale in translations)
            {
                locales.Add(locale.name, new Dictionary<string, string> { });

                int maxLength = locale.text.Split(char.Parse("\n")).Length;
                int i = 0;
                foreach (string line in locale.text.Split(char.Parse(";")))
                {
                    if (i == maxLength)
                        break;

                    if (line.Replace("\n", "").Replace("\r", "").Length == 0)
                        continue;

                    if (line.Replace("\n", "").Replace("\r", "")[0] == char.Parse("#"))
                        continue;

                    var l = line.Replace(" = ", "=");

                    string key = l.Split(char.Parse("="))[0].Replace("\n", "").Replace("\r", "");
                    string value = l.Split(char.Parse("="))[1].Replace("<semicolon />", ";");

                    locales[locale.name].Add(key, value);
                    i++;
                }
            }

            return locales;
        }

        private static List<TextMeshProUGUI> GetText()
        {
            List<TextMeshProUGUI> toTranslate = new List<TextMeshProUGUI> { };
            foreach (Transform @object in GameObject.Find("Canvas").transform)
                GetChildren(@object.gameObject, ref toTranslate);
            return toTranslate;
        }

        private static void GetChildren(GameObject parent, ref List<TextMeshProUGUI> toTranslate)
        {
            if (parent.GetComponent<TextMeshProUGUI>() != null)
                toTranslate.Add(parent.GetComponent<TextMeshProUGUI>());

            foreach (Transform child in parent.transform)
            {
                if (child.GetComponent<TextMeshProUGUI>() != null)
                    toTranslate.Add(child.GetComponent<TextMeshProUGUI>());
                GetChildren(child.gameObject, ref toTranslate);
            }
        }

        public static string Get(string key) => GetLocales().ContainsKey(CurrentLocale) ? GetLocales()[CurrentLocale][key] : GetLocales()["en_gb"][key];

    }
}
