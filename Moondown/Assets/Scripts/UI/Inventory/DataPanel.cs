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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Moondown.UI.Inventory
{
    public static class DataPanel
    {
        public static string Title { get; set; }
        public static string SubHeading { get; set; }
        public static Texture Image { get; set; }

        private static GameObject panel;

        /////////////////////////////////////////////

        private static TextMeshProUGUI titleText;
        private static TextMeshProUGUI subHeadingText;
        private static RawImage        image;

        public static void Init(GameObject a_Panel, TextMeshProUGUI a_TitleText, TextMeshProUGUI a_SubHeadingText, RawImage a_Image)
        {
            panel = a_Panel;
            titleText = a_TitleText;
            subHeadingText = a_SubHeadingText;
            image = a_Image;
            
        }

        public static void Show()
        {
            titleText.text = Title;
            subHeadingText.text = SubHeading;
            image.texture = Image;

            panel.SetActive(true);
        }

        public static void Hide()
        {
            panel.SetActive(false);
        }

    }
}