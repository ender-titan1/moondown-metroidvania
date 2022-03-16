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

using Moondown.UI.Inventory;

namespace Moondown.UI
{

    public class UIInput : MonoBehaviour
    {
        public static UIInput Instance { get; private set; }

        private MainControls controls;

        [Header("Inventory")]
        [SerializeField] private GameObject UI;
        [SerializeField] private GameObject inventoryPostProcessing;

        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseUI;

        public bool isInInventory;
        private bool isInPasue;

        public GameObject InventoryUI => UI;

        private void Awake()
        {
            if (Instance == null)
                Instance  = this;
            else
                Destroy(gameObject);

            controls = new MainControls();

            controls.Player.Interact.performed += _ => Interact();
            controls.Player.OpenInventory.performed += _ => OpenInventoryUI();
            controls.Player.PauseandexitUI.performed += _ => PauseOrExitUI();
            controls.Player.Pause.performed += _ =>
            {
                if (isInInventory)
                {
                    UI.SetActive(false);
                    isInInventory = false;
                }

                if (!isInPasue)
                {
                    pauseUI.SetActive(true);
                    Time.timeScale = 0;
                    isInPasue = true;
                }
                else
                {
                    pauseUI.SetActive(false);
                    Time.timeScale = 1;
                    isInPasue = false;
                }
            };

            controls.Player.ExitUI.performed += _ =>
            {
                if (isInInventory)
                {
                    if (!InventoryNavigation.Instance.SideBarActive)
                    {
                        InventoryNavigation.Instance.SideBarActive = true;
                        InventoryNavigation.Instance.selectedSlot.OnPointerExit(null);
                        return;
                    }

                    UI.SetActive(false);
                    inventoryPostProcessing.SetActive(false);
                    DisplayHUD.Toggle();
                    isInInventory = false;
                }
                else if (isInPasue)
                {
                    pauseUI.SetActive(false);
                    Time.timeScale = 1;
                    isInPasue = false;
                }
            };

            controls.Enable();
        }

        void PauseOrExitUI()
        {
            if (isInInventory)
            {
                if (!InventoryNavigation.Instance.SideBarActive)
                {
                    DataPanel.Hide();
                    InventoryNavigation.Instance.SideBarActive = true;
                    InventoryNavigation.Instance.selectedSlot.OnPointerExit(null);
                    return;
                }

                UI.SetActive(false);
                inventoryPostProcessing.SetActive(false);
                DisplayHUD.Toggle();
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
                inventoryPostProcessing.SetActive(true);
                UI.SetActive(true);
                DisplayHUD.Toggle();
                isInInventory = true;
            }
            else
            {
                inventoryPostProcessing.SetActive(false);
                UI.SetActive(false);
                DisplayHUD.Toggle();
                isInInventory = false;
            }
        }
    }
}