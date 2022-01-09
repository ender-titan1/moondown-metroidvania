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
using Moondown.UI.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Moondown.UI
{

    public class UIInput : MonoBehaviour
    {
        private MainControls controls;

        [Header("Inventory")]
        [SerializeField] private GameObject[] slots;
        [SerializeField] private GameObject[] quickBarSlots;
        [SerializeField] private GameObject equipedWeaponSlot;
        [SerializeField] private Sprite baseSlotSprite;
        [SerializeField] private GameObject UI;

        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseUI;

        private bool isInInventory;
        private bool isInPasue;

        private void Awake()
        {
            DisplayInventory.Instance.slots = slots;
            DisplayInventory.Instance.quickSelectSlots = quickBarSlots;

            controls = new MainControls();

            controls.Player.Interact.performed += _ => Interact();
            controls.Player.OpenInventory.performed += _ => OpenInventoryUI();
            controls.Player.PauseandexitUI.performed += _ => PauseOrExitUI();

            controls.Enable();
        }

        void PauseOrExitUI()
        {
            if (isInInventory)
            {
                UI.SetActive(false);
                isInInventory = false;
            }
            else if (isInPasue)
            {
                pauseUI.SetActive(false);
                Time.timeScale = 1;
                isInPasue = false;
            }
            else
            {
                pauseUI.SetActive(true);
                Time.timeScale = 0;
                isInPasue = true;
            }
        }

        void Interact()
        {

        }

        void OpenInventoryUI()
        {
            if (!isInInventory)
            {
                DisplayInventory.Instance.Load(slots, quickBarSlots, baseSlotSprite, UI, equipedWeaponSlot);
                isInInventory = true;
            }
            else
            {
                UI.SetActive(false);
                isInInventory = false;
            }
        }
    }
}