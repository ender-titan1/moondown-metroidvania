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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Moondown.UI.Inventory
{
    public class InventoryNavigation : MonoBehaviour
    {
        public enum Direction
        {
           Up,
           Down,
           Left,
           Right
        }

        private const int ROW_LENGTH = 6;

        public static InventoryNavigation Instance { get; set; }

        private MainControls inputs;

        [SerializeField] private GameObject[] sideBar;
        private GameObject selected;
        private int selectedIndex = 0;

        public bool SideBarActive { get; set; } = true;
        
        ////////////////////////////////////////////////////////

        public List<List<Slot>> slots = new List<List<Slot>>();

        private int row = 0;
        private int col = 0;

        public Slot selectedSlot;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            inputs = new MainControls();

            inputs.UI.Down.performed +=   _ => Move(Direction.Down);
            inputs.UI.Up.performed +=     _ => Move(Direction.Up);
            inputs.UI.Left.performed +=   _ => Move(Direction.Left);
            inputs.UI.Right.performed +=  _ => Move(Direction.Right);
            inputs.UI.Select.performed += _ => SelectPage();

        }

        private void OnEnable()
        {
            inputs.Enable();
            GenSlots();

            Clear(null);

            selectedIndex = 0;
            Select(selectedIndex);
        }

        private void GenSlots()
        {
            slots.Clear();

            GameObject[] objects = gameObject.GetChildren();

            slots.Add(new List<Slot>());

            int count = 0;
            int row = 0;
            foreach (GameObject gameObject in objects)
            {
                if (!gameObject.Has<Slot>())
                    continue;

                slots[row].Add(gameObject.GetComponent<Slot>());


                if (count == ROW_LENGTH)
                {
                    row++;
                    slots.Add(new List<Slot>());
                }

                count++;
            }

        }

        private void OnDisable()
        {
            selected.GetComponentInChildren<Animator>().SetBool("Mouse off", true);
            DataPanel.Hide();

            inputs.Disable();
        }

        public void Clear(GameObject button)
        {
            for (int i = 0; i < sideBar.Length; i++)
            {
                if (sideBar[i] == button)
                    continue;

                gameObject.GetComponent<DisplayInventory>().OnButtonExit(sideBar[i].GetComponentInChildren<Animator>());
            }
        }

        private void SelectPage()
        {
            if (!SideBarActive)
                return;

            // Side Bar
            sideBar[selectedIndex].GetComponent<Button>().onClick.Invoke();
            SideBarActive = false;

            slots[0][0].SetColor(1, 0.11461f, 0);
            selectedSlot = slots[0][0];
            row = 0;
            col = 0;

            if (selectedSlot.content != null)
            {
                DataPanel.Title = selectedSlot.content.Value.item.Name;
                DataPanel.Image = selectedSlot.content.Value.item.data.image;
                DataPanel.SubHeading =
                    selectedSlot.content.Value.item.data.rarity.ToString().ToLower().CapitalizeFirst() +
                    " " +
                    selectedSlot.content.Value.item.data.type.ToString().ToLower().Replace("_", " ");
                DataPanel.Show();
            }
            else
            {
                DataPanel.Hide();
            }
        }

        private void Move(Direction dir)
        {
            if (SideBarActive)
            {

                if (dir == Direction.Down)
                {
                    if (sideBar[5] == selected)
                    {
                        Select(0);
                        selectedIndex = 0;
                    }
                    else
                    {
                        selectedIndex++;
                        Select(selectedIndex);
                    }
                }
                else if (dir == Direction.Up)
                {
                    if (sideBar[0] == selected)
                    {
                        Select(5);
                        selectedIndex = 5;
                    }
                    else
                    {
                        selectedIndex--;
                        Select(selectedIndex);
                    }
                }
            }
            else
            {

                switch (dir)
                {
                    case Direction.Up:
                        row--;
                        SelectSlot(row, col, row + 1, col);
                        break;
                    case Direction.Down:
                        row++;
                        SelectSlot(row, col, row - 1, col);
                        break;
                    case Direction.Left:
                        col--;
                        SelectSlot(row, col, row, col + 1, true);
                        break;
                    default:
                        col++;
                        SelectSlot(row, col, row, col - 1);
                        break;
                }
            }
        }

        private void SelectSlot(int r, int c, int pr, int pc, bool left = false)
        {
            Slot slot = null;

            try
            { 
                selectedSlot.OnPointerExit(null);
                slot = selectedSlot;
            }
            catch (NullReferenceException) { }

            try
            {
                selectedSlot = slots[r][c];
                slots[r][c].SetColor(1, 0.11461f, 0);
            }
            catch (ArgumentOutOfRangeException)
            {
                selectedSlot = slot;
                slot.SetColor(1, 0.11461f, 0);
                row = pr; // previous row
                col = pc; // previous column

                if (left && col == 0)
                {
                    SideBarActive = true;
                    selectedSlot.OnPointerExit(null);
                    DataPanel.Hide();
                }
            }

            if (selectedSlot.content != null)
            {
                DataPanel.Title = selectedSlot.content.Value.item.Name;
                DataPanel.Image = selectedSlot.content.Value.item.data.image;
                DataPanel.SubHeading =
                    selectedSlot.content.Value.item.data.rarity.ToString().ToLower().CapitalizeFirst() +
                    " " +
                    selectedSlot.content.Value.item.data.type.ToString().ToLower().Replace("_", " ");
                DataPanel.Show();
            }
            else
            {
                DataPanel.Hide();
            }
        }


        private void Select(int index)
        {
            selected = sideBar[index];

            DisplayInventory display = gameObject.GetComponent<DisplayInventory>();
            display.OnButtonEnter(sideBar[index].GetComponentInChildren<Animator>());
            display.OnSwitchPage(sideBar[index].name.Split(char.Parse(" "))[0]);

            for (int i = 0; i < sideBar.Length; i++)
            {
                if (i == index)
                    continue;

                display.OnButtonExit(sideBar[i].GetComponentInChildren<Animator>());
            }
        }
    }
}
