/*
    A script to control basic player movement
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
using UnityEngine;
using UnityEngine.EventSystems;

namespace Moondown.UI
{
    public class NavBarElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public void Select(bool value)
        {
            if (value)
                GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1, 0.11461f, 0);
            else
                GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        }

        public void Activate()
        {
            if (gameObject.GetComponent<EventTrigger>() != null)
            {
                gameObject.GetComponent<EventTrigger>().OnPointerDown(null);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            VerticalNavBar navBar = GetComponentInParent<VerticalNavBar>();
            navBar.HideAll();
            navBar.SelectEvent();
            Select(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Select(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GetComponentInParent<VerticalNavBar>().Activate();
        }
    }
}