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

using Moondown.UI.Localization;
using Moondown.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Moondown.Inventory
{
    public class Item 
    {
        public enum Rarity
        {
            None = 0,
            Uncommon = 1,
            Rare = 2,
            Uniqe = 3,
            Special = 4
        }

        [System.Flags]
        public enum Properties
        {
            None = 0,
            Flamable = 1,
            Heavy = 2
        }

        public ItemData data;
        public string Name { get; protected set; }
        public string Desc { get; protected set; }

        public Item(string name)
        {
            this.data = Resources.Load<ItemData>(@"Inventory\" + name);
            Name = LocalizationManager.Get(data.nameKey);
            Desc = LocalizationManager.Get(data.descriptionKey);
        }

        public void RefreshName()
        {
            Name = LocalizationManager.Get(data.nameKey);
            Desc = LocalizationManager.Get(data.descriptionKey);
        }
    }
}