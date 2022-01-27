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
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Moondown.Inventory
{
    public class Item : IInventoryItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public int SlotNumber { get; set; }
        public Sprite Image { get; set; }
        public Sprite ImageWithSlot { get; set; }

        public Item(string name, string desc, string spriteName, ItemType type, int slotNumber)
        {
            this.Name = name;
            this.Description = desc;

            this.Type = type;

            string spritePath = @"Assets/Graphics/Sprites/" + spriteName + ".png";
            this.Image = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            Sprite baseSprite = AssetDatabase.LoadAssetAtPath<Sprite>(@"Assets/Graphics/Sprites/UI/Inventory/Inventory.png");
            this.ImageWithSlot = baseSprite.MergeSprites(Image);

            this.SlotNumber = slotNumber;
        }

    }
}