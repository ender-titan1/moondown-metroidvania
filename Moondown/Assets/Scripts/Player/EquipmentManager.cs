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
using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }
    public string MeeleWeaponName { get; private set; }
    public Weapon EquipedWeapon { get; private set; }

    public bool FirstLoading { private get; set; } = true;

    int  _current = 0;

    public int NextFreeSlot
    {
        get
        {
            if (FirstLoading)
            {
                int val = _current;
                _current++;
                return val;

            }


            for (int i = 0; i < InventoryDisplay.Instance.slots.Length; i++)
            {
                GameObject slot = InventoryDisplay.Instance.slots[i];


                if (slot.GetComponent<Slot>().item == null)
                    return i;
            }

            return 0;
        }
    }

    public int InvSize => InventoryDisplay.Instance.slots.Length;

    public List<IInventoryItem> Inventory { get; set; } = new List<IInventoryItem> { };

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Equip(string name)
    {
        EquipedWeapon = (Weapon)(
            from IInventoryItem item in Inventory
            where item.Name == name
            select item
        ).First();

        MeeleWeaponName = name;
    }

    public void Equip(Weapon weapon)
    {

        EquipedWeapon = (Weapon)(
            from IInventoryItem item in Inventory
            where ((Weapon)item).Name == weapon.Name
            select item
        ).First();

        MeeleWeaponName = weapon.Name;
    }

    public void UnequipWeapon() { MeeleWeaponName = null; EquipedWeapon = null; }
}
