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
using UnityEngine;

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

        private GameObject active;
        private MainControls inputs;

        [SerializeField] private GameObject[] sideBar;
        private GameObject selected;
        private int selectedIndex = 0;

        private bool sideBarActive = true;

        private void Awake()
        {
            inputs = new MainControls();

            inputs.UI.Down.performed +=  _ => Move(Direction.Down);
            inputs.UI.Up.performed +=    _ => Move(Direction.Up);
            inputs.UI.Left.performed +=  _ => Move(Direction.Left);
            inputs.UI.Right.performed += _ => Move(Direction.Right);
        }

        private void OnEnable()
        {
            Select(0, 5);
            inputs.Enable();
        }

        private void OnDisable() => inputs.Disable();

        private void Move(Direction dir)
        {
            if (sideBarActive)
            {
                if (dir == Direction.Down)
                {
                    if (sideBar[5] == selected)
                    {
                        Select(0, 5);
                        selectedIndex = 0;
                    }
                    else
                    {
                        Select(selectedIndex += 1, selectedIndex - 1);
                    }
                }
                else if (dir ==  Direction.Up)
                {
                    if (sideBar[0] == selected)
                    {
                        Select(5, 0);
                        selectedIndex = 5;
                    }
                    else
                    {
                        Select(selectedIndex -= 1, selectedIndex + 1);
                    }
                }
            }
        }

        private void Select(int index, int previousIndex)
        {
            selected = sideBar[index];

            DisplayInventory display = gameObject.GetComponent<DisplayInventory>();
            display.OnButtonEnter(sideBar[index].GetComponentInChildren<Animator>());

            display.OnButtonExit(sideBar[previousIndex].GetComponentInChildren<Animator>());
        }
    }
}
