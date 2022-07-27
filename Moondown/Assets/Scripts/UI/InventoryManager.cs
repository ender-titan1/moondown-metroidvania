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
using Moondown.UI;
using Moondown.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VectorGraphics;
using UnityEngine;

namespace Moondown.Inventory
{
    public partial class InventoryManager
    {
        VerticalNavBar navBar = null;
        NavGrid navGrid = null;
        MainControls controls;
        bool subscribed = false;

        public void OnInventoryOpen()
        {
            if (navBar == null)
                navBar = GameObject.FindGameObjectWithTag("InvNavBar").GetComponent<VerticalNavBar>();

            if (navGrid == null)
                navGrid = GameObject.FindGameObjectWithTag("InvNavGrid").GetComponent<NavGrid>();

            if (!subscribed)
            {
                navBar.OnSelect += OnNavBarSelect;
                navBar.OnActivate += OnNavBarActivate;
                navGrid.OnSelect += OnNavGridSelect;
                navGrid.OnPreSelect += OnNavGridPreSelect;
                subscribed = true;
                navBar.Enabled = true;
            }

            controls = new MainControls();
        }

        public void OnInventoryClose()
        {
            if (subscribed)
            {
                navBar.OnSelect -= OnNavBarSelect;
                navBar.OnActivate -= OnNavBarActivate;
                navGrid.OnSelect -= OnNavGridSelect;
                navGrid.OnPreSelect -= OnNavGridPreSelect;
                subscribed = false;
                navBar.Enabled = false;
            }
        }

        private void OnNavBarSelect(GameObject selected)
        {
            if (selected == null)
                return;

            if (!navBar.Enabled)
            {
                navGrid.Enabled = false;
                navBar.Enabled = true;
            }

            PropertyInfo prop = typeof(InventoryManager).GetProperty(selected.name);
            List<ItemStack> inv = Instance.GetInventory((List<ItemStack>)prop.GetValue(Instance), null);
            navGrid.LoadPanel(inv);
        }

        private void OnNavBarActivate(GameObject selected)
        {
            navGrid.Enabled = navBar.Enabled;
            navBar.Enabled = !navBar.Enabled;
        }

        private void OnNavGridSelect(NavGrid.GridSlot slot)
        {
            slot.slot.GetComponent<SVGImage>().color = new Color(1, 0.11461f, 0);
            Debug.Log(slot.pos);
        }

        private void OnNavGridPreSelect(NavGrid.GridSlot slot)
        {
            slot.slot.GetComponent<SVGImage>().color = Color.white;
        }

        public void HandleInventoryToggle(bool value)
        {
            if (value)
                OnInventoryOpen();
            else
                OnInventoryClose();
        }
    }
}
