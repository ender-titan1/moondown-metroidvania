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

namespace Moondown.Inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/Item", order = 100)]
    public class ItemData : ScriptableObject
    {
        public string nameKey;
        public string descriptionKey;

        public Texture2D image;

        public ItemType type;

        public int stackSize;
        public Item.Rarity rarity;

    }
}