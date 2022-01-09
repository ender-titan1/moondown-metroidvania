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
using Moondown.WeaponSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Moondown.UI.Inventory
{

    [RequireComponent(typeof(RawImage))]
    public class Slot : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector]
        public IInventoryItem item = null;

        public void OnClick(Sprite @base)
        {
            if (item == null)
                return;

            if (item.Type == ItemType.MEELE_WEAPON)
            {
                GameObject s = DisplayInventory.Instance.equipedWeaponSlot;

                if (s == gameObject)
                {
                    Weapon weapon = (Weapon)item;

                    s.GetComponent<RawImage>().enabled = false;
                    s.GetComponent<RawImage>().texture = null;
                    s.GetComponent<Slot>().item = null;

                    EquipmentManager.Instance.UnequipWeapon();

                    weapon.SlotNumber = EquipmentManager.Instance.NextFreeSlot;
                    DisplayInventory.Instance.Load();

                    return;
                }

                Texture2D texture = item.Image.texture;
                IInventoryItem selectedItem = item;

                Slot[] slots = (from GameObject slot in DisplayInventory.Instance.allSlots
                                where slot.GetComponent<Slot>().item == item
                                select slot.GetComponent<Slot>()
                               ).ToArray();


                foreach (Slot slot in slots)
                {
                    slot.gameObject.GetComponent<RawImage>().texture = @base.texture;
                    slot.item = null;
                }


                s.GetComponent<RawImage>().enabled = true;
                s.GetComponent<RawImage>().texture = texture;
                s.GetComponent<Slot>().item = selectedItem;
                EquipmentManager.Instance.Equip((Weapon)selectedItem);
                selectedItem.SlotNumber = -1;

            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnClick(DisplayInventory.Instance.baseSlotTexture);
            }
        }
    }
}