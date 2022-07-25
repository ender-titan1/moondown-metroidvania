/*
    A script to control basic player movement
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
using Moondown.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.UI
{
    public class NavGrid : MonoBehaviour
    {
        public struct GridSlot
        {
            public static GridSlot Empty => new GridSlot(null, Vector2Int.zero, null);

            public ItemStack? stack;
            public Vector2Int pos;
            public GameObject slot;

            public GridSlot(ItemStack? stack, Vector2Int pos, GameObject slot)
            {
                this.stack = stack;
                this.pos = pos;
                this.slot = slot;
            }
        }

        public const int COLUMNS_PER_ROW = 5;
        public const int MAX_COLUMNS = 10;

        private GridSlot selected;
        private MainControls controls;
        private bool _enabled;
        private GridSlot[][] selection;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;

                if (value)
                    controls.Enable();
                else
                    controls.Disable();
            }
        }

        public void Awake()
        { 
            int rows = transform.childCount / COLUMNS_PER_ROW;
            selection = new GridSlot[rows][];

            int child = 0;
            for (int y = 0; y < rows; y++)
            {
                selection[y] = new GridSlot[COLUMNS_PER_ROW] { GridSlot.Empty, GridSlot.Empty, GridSlot.Empty, GridSlot.Empty, GridSlot.Empty };

                for (int x = 0; x < COLUMNS_PER_ROW; x++)
                {
                    selection[y][x] = new GridSlot(null, new Vector2Int(x, y), transform.GetChild(child).gameObject);
                    child++;
                }
            }

            controls = new MainControls();
            controls.UI.Select.performed += _ => Debug.Log("hi");
        }

        public void LoadPanel(ItemStack[] inv)
        {

        }
    }
}