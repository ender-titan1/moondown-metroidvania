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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moondown.Inventory
{
    public partial class InventoryManager
    {
        public static InventoryManager Instance { get; set; } = new InventoryManager();

        public List<ItemStack> Resources { get; set; } = new List<ItemStack>();
        public List<ItemStack> Weapons   { get; set; } = new List<ItemStack>();
        public List<ItemStack> Armour    { get; set; } = new List<ItemStack>();
        public List<ItemStack> Tools     { get; set; } = new List<ItemStack>();
        public List<ItemStack> Modules   { get; set; } = new List<ItemStack>();
        public List<ItemStack> Special   { get; set; } = new List<ItemStack>();

        public List<ItemStack> All
        {
            get
            {
                List<ItemStack> items = new List<ItemStack>();
                items.AddRange(Resources);
                items.AddRange(Weapons);
                items.AddRange(Armour);
                items.AddRange(Tools);
                items.AddRange(Modules);
                items.AddRange(Special);
                return items;
            }
        }

        public void Add(Item item, int amount = 1)
        {
            ItemStack stack = new ItemStack(item, amount);

            if (item.data.rarity == Item.Rarity.Special)
            {
                Special.Add(stack);
                return;
            }

            switch (item.data.type)
            {
                case ItemType.Melee_Weapon:
                case ItemType.Ranged_Weapon:
                    Weapons.Add(stack);
                    break;
                case ItemType.Item:
                    Resources.Add(stack);
                    break;
                case ItemType.Armour:
                    Armour.Add(stack);
                    break;
                case ItemType.Tool:
                    Tools.Add(stack);
                    break;
                case ItemType.Module:
                    Modules.Add(stack);
                    break;
                default:
                    Resources.Add(stack);
                    break;
            }
        }

        public List<ItemStack> GetInventory(List<ItemStack> stacks, string filter)
        {
            List<ItemStack> inventory = new List<ItemStack>();
            Dictionary<string, int> itemCounts = new Dictionary<string, int>();

            // Get count of all the items
            foreach (ItemStack stack in stacks)
            {
                string key = stack.item.data.name;
                if (itemCounts.ContainsKey(key))
                    itemCounts[key] += stack.amount;
                else
                    itemCounts.Add(key, stack.amount);
            }
            
            // Create stacks
            foreach (KeyValuePair<string, int> pair in itemCounts)
            {
                Item item = new Item(pair.Key);
                int value = pair.Value;
                int size = item.data.stackSize;
                int stackAmount = value / size;
                int remainder = value % size;
                int valuePerStack = (value - remainder) / stackAmount;
                
                for (int i = 0; i < stackAmount; i++)
                    inventory.Add(new ItemStack(item, valuePerStack));

                if (remainder != 0)
                    inventory.Add(new ItemStack(item, remainder));
            }

            filter ??= "";
            inventory = (from ItemStack stack in inventory
                         where stack.item.Name.ToLower().Contains(filter.ToLower())
                         orderby (int)stack.item.data.rarity descending
                         select stack).ToList();

            return inventory;

        }
    }
}
