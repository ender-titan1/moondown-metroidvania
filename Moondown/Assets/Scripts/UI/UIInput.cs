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
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    private MainControls controls;

    [SerializeField] private GameObject[] slots;
    [SerializeField] private GameObject[] quickBarSlots;
    [SerializeField] private GameObject equipedWeaponSlot;
    [SerializeField] private Sprite baseSlotSprite;
    [SerializeField] private GameObject UI;

    private bool isInInventory;

    private void Awake()
    {
        InventoryDisplay.Instance.slots = slots;
        InventoryDisplay.Instance.quickSelectSlots = quickBarSlots;

        controls = new MainControls();

        controls.Player.Interact.performed += _ => Interact();
        controls.Player.OpenInventory.performed += _ => OpenInventoryUI();
        controls.Enable();
    }

    void Interact()
    {

    }

    void OpenInventoryUI()
    {
        if (!isInInventory) { 
            InventoryDisplay.Instance.Load(slots, quickBarSlots, baseSlotSprite, UI, equipedWeaponSlot);
            isInInventory = true;
        }   
        else
        {
            UI.SetActive(false);
            isInInventory = false;
        }
    }
}
