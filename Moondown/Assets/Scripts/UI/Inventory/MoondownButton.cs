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
using Moondown.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Moondown.UI
{
    public class MoondownButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color color;
        private Color original;

        public void OnEnable()
        {
            if (gameObject.ChildHas<Text>())
                original = gameObject.GetComponentInChildren<Text>().color;
            else if (gameObject.ChildHas<TextMeshProUGUI>())
                original = gameObject.GetComponentInChildren<TextMeshProUGUI>().color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (gameObject.ChildHas<Text>())
                gameObject.GetComponentInChildren<Text>().color = color;
            else if (gameObject.ChildHas<TextMeshProUGUI>())
                gameObject.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (gameObject.ChildHas<Text>())
                gameObject.GetComponentInChildren<Text>().color = original;
            else if (gameObject.ChildHas<TextMeshProUGUI>())
                gameObject.GetComponentInChildren<TextMeshProUGUI>().color = original;
        }
    }
}
