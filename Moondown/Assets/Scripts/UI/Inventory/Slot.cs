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
using Moondown.Inventory;
using Moondown.Player.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Moondown.UI.Inventory
{
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemStack? content;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryNavigation.Instance.SideBarActive)
                return;
                        
            SetColor(1, 0.11461f, 0);

            if (content != null)
                TooltipManager.Instance.ShowTooltip(content.Value.item.Name);
            else
                TooltipManager.Instance.HideTooltip();

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.HideTooltip();

            SetColor(1, 1, 1, false);
        }

        public void SetColor(float r, float g, float b, bool enter=true)
        {
            if (enter)
            {
                foreach (Slot slot in InventoryNavigation.Instance.slots.SelectMany(x => x).ToList())
                {
                    slot.OnPointerExit(null);
                }
            }

            gameObject.GetComponent<SVGImage>().color = new Color(r, g, b);
        }

    }
}
