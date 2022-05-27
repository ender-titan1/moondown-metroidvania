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
using System;

namespace Moondown.UI
{

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private MainControls controls;

        [Header("Inventory")]
        [SerializeField] private GameObject UI;
        [SerializeField] private GameObject UIPostProcessing;

        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseUI;

        public bool isInInventory;
        private bool isInPasue;

        public bool IsInInterface
        {
            get => isInInventory || isInPasue;
        }

        public GameObject InventoryUI => UI;

        private void Awake()
        {
            if (Instance == null)
                Instance  = this;
            else
                Destroy(gameObject);

            controls = new MainControls();

            controls.Player.OpenInventory.performed += _ => OpenInventoryUI();
            controls.Player.PauseandexitUI.performed += _ => PauseOrExitUI();
            controls.Player.Pause.performed += _ =>
            {
                if (isInInventory)
                {
                    UI.SetActive(false);
                    isInInventory = false;
                }

                TogglePause(!isInPasue);
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

                    ToggleInventory(false);
                }
                else if (isInPasue)
                {
                    TogglePause(false);
                }
            };

            controls.Enable();
        }

        private void PauseOrExitUI()
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

                ToggleInventory(false);
            }
            else
            {
                TogglePause(!isInPasue);
            }
        }

        private void OpenInventoryUI()
        {
            if (isInPasue)
                return;

            if (!isInInventory)
                ToggleInventory(true);
            else
                ToggleInventory(false);
        }

        private void TogglePause(bool value)
        {
            DisplayHUD.Toggle(!value);
            pauseUI.SetActive(value);
            Time.timeScale = Convert.ToInt32(!value);
            isInPasue = value;
            UIPostProcessing.SetActive(value);
            pauseUI.GetComponentInChildren<VerticalNavBar>().Enabled = value;
        }

        private void ToggleInventory(bool value)
        {
            UIPostProcessing.SetActive(value);
            UI.SetActive(value);
            DisplayHUD.Toggle(!value);
            isInInventory = value;
        }


        #region Pause Menu

        public void PauseQuit()
        {
            DisablePauseNav();
            //TODO: add confirmation window
            Application.Quit();
        }

        public void OpenSettings()
        {
            //TODO: add settings
        }

        public void Resume()
        {
            DisablePauseNav();
            TogglePause(false);
        }

        private void DisablePauseNav()
        {
            pauseUI.GetComponentInChildren<VerticalNavBar>().Enabled = false;
        }

        #endregion
    }
}