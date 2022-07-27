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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Moondown.UI
{
    public class NavGrid : MonoBehaviour
    {
        public struct GridSlot
        {
            public static GridSlot Empty => new GridSlot(null, Vector2Int.zero, null, 0);

            public ItemStack? stack;
            public Vector2Int pos;
            public GameObject slot;
            public int index;

            public GridSlot(ItemStack? stack, Vector2Int pos, GameObject slot, int index)
            {
                this.stack = stack;
                this.pos = pos;
                this.slot = slot;
                this.index = index;
            }
        }
        
        public event Action<GridSlot> OnSelect;
        public event Action<GridSlot> OnPreSelect;
        public event Action<GridSlot> OnActivate;

        public const int COLUMNS_PER_ROW = 6;

        private int xAxis, yAxis;
        private GridSlot? selected;
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
                {
                    Select();
                    controls.Enable();
                }
                else
                {
                    controls.Disable();
                }
            }
        }

        public void Awake()
        { 
            int rows = transform.childCount / COLUMNS_PER_ROW;
            selection = new GridSlot[rows][];

            int child = 0;
            for (int y = 0; y < rows; y++)
            {
                selection[y] = new GridSlot[COLUMNS_PER_ROW] { GridSlot.Empty, GridSlot.Empty, GridSlot.Empty, GridSlot.Empty, GridSlot.Empty, GridSlot.Empty };

                for (int x = 0; x < COLUMNS_PER_ROW; x++)
                {
                    selection[y][x] = new GridSlot(null, new Vector2Int(x, y), transform.GetChild(child).gameObject, child);
                    child++;
                }
            }

            controls = new MainControls();
            controls.UI.Select.performed += _ => Debug.Log("hi");

            controls.UI.Left.performed += _ =>
            {
                xAxis--;
                Select();
            };

            controls.UI.Right.performed += _ => 
            {
                xAxis++;
                Select();
            };

            controls.UI.Up.performed += _ => 
            {
                yAxis++;
                Select();
            };

            controls.UI.Down.performed += _ => 
            {
                yAxis--;
                Select();
            };

            controls.UI.Left.canceled += _ => xAxis++;
            controls.UI.Right.canceled += _ => xAxis--;
            controls.UI.Up.canceled += _ => yAxis--;
            controls.UI.Down.canceled += _ => yAxis++;
        }

        public void Select()
        {
            if (selected == null)
            {
                selected = selection[0][0];
                OnSelect?.Invoke(selected.Value);
                return;
            }

            OnPreSelect?.Invoke(selected.Value);
            
            try
            {
                selected = selection[selected.Value.pos.y + yAxis * -1][selected.Value.pos.x + xAxis * 1];
            }
            catch (IndexOutOfRangeException)
            {

            }

            OnSelect?.Invoke(selected.Value);
        }

        public Vector2Int NextFree()
        {
            return (from GridSlot slot in selection.SelectMany(x => x)
                    where slot.stack == null
                    orderby slot.index ascending
                    select slot.pos).First();
        }

        public void UnloadAll()
        {
            foreach (GridSlot slot in selection.SelectMany(x => x))
            {
                slot.slot.GetComponentInChildren<RawImage>().enabled = false;
                slot.slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
                selection[slot.pos.y][slot.pos.x].stack = null;
            }
        }

        public void LoadPanel(List<ItemStack> inv)
        {
            UnloadAll();

            foreach (ItemStack stack in inv)
            {
                Vector2Int pos = NextFree();
                ref GridSlot gridSlot = ref selection[pos.y][pos.x];

                gridSlot.stack = stack;
                RawImage image = gridSlot.slot.GetComponentInChildren<RawImage>();
                TextMeshProUGUI text = gridSlot.slot.GetComponentInChildren<TextMeshProUGUI>();

                image.enabled = true;
                image.texture = stack.item.data.image;

                text.text = stack.amount != 1 ? stack.amount.ToString() : "";
            }
        }
    }
}