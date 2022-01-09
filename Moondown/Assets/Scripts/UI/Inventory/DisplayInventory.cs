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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Moondown.UI.Inventory
{
    public class DisplayInventory
    {
        enum LastViewed
        {
            MEELE_WEAPON,
            RANGED_WEAPON,
            ARMOUR,
            GENERAL
        }

        public static DisplayInventory Instance { get; private set; } = new DisplayInventory();

        private LastViewed lastViewed = LastViewed.MEELE_WEAPON;

        public GameObject[] slots;
        public GameObject[] quickSelectSlots;
        public GameObject equipedWeaponSlot;
        public GameObject[] allSlots;
        public Sprite baseSlotTexture;
        public GameObject UI;

        public void Load(GameObject[] slots, GameObject[] quickBarSlots, Sprite baseSprite, GameObject UI, GameObject equipedWeaponSlot)
        {
            this.equipedWeaponSlot = equipedWeaponSlot;
            this.quickSelectSlots = quickBarSlots;
            this.slots = slots;
            this.baseSlotTexture = baseSprite;
            this.UI = UI;

            List<GameObject> allSlots = slots.ToList();
            allSlots.AddRange(quickBarSlots);
            this.allSlots = allSlots.ToArray();

            for (int i = 0; i < slots.Length; i++)
            {
                IInventoryItem item = null;

                try
                {
                    item = (
                        from IInventoryItem it in EquipmentManager.Instance.Inventory
                        where it.SlotNumber == i
                        select it
                    ).ToArray().First();

                }
                catch (Exception)
                {
                    continue;
                }


                slots[i].GetComponent<RawImage>().texture = item.ImageWithSlot.texture;
                slots[i].GetComponent<Slot>().item = item;

            }



            if (lastViewed == LastViewed.MEELE_WEAPON)
                LoadRelevantItems(quickBarSlots, baseSprite, ItemType.MEELE_WEAPON, ItemType.TOOL);

            UI.SetActive(true);
        }

        public void Load()
        {
            Load(slots, quickSelectSlots, baseSlotTexture, UI, equipedWeaponSlot);
        }

        private void LoadRelevantItems(GameObject[] slots, Sprite baseSprite, params ItemType[] types)
        {
            List<IInventoryItem> items = new List<IInventoryItem> { };

            for (
                int i = 0;
                i < slots.Length &&
                i < EquipmentManager.Instance.Inventory.ToArray().Length;
                i++
            )
            {
                IInventoryItem item = EquipmentManager.Instance.Inventory[i];

                if (types.Contains(item.Type))
                    items.Add(item);
            }


            foreach (GameObject slot in slots)
            {
                if (items.ToArray().Length == 0)
                    break;

                IInventoryItem item = items.ToArray().First();

                slot.GetComponent<RawImage>().texture = item.ImageWithSlot.texture;
                slot.GetComponent<Slot>().item = item;

                items.RemoveAt(0);
            }

        }


    }
}