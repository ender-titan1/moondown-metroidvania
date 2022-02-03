using Moondown.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moondown.Inventory
{
    public class InventoryManager
    {
        public static InventoryManager Instance { get; set; } = new InventoryManager();

        public List<Item> Resources { get; set; }  = new List<Item>();
        public List<Item> Weapons   { get; set; }  = new List<Item>();
        public List<Item> Armour    { get; set; }  = new List<Item>();
        public List<Item> Tools     { get; set; }  = new List<Item>();
        public List<Item> Modules   { get; set; }  = new List<Item>();
        
        public void Add(Item item)
        {
            switch (item.data.type)
            {
                case ItemType.MEELE_WEAPON:
                case ItemType.RANGED_WEAPON:
                    Weapons.Add(item);
                    break;
                case ItemType.TOOL:
                    Tools.Add(item);
                    break;
                case ItemType.ARMOUR:
                    Armour.Add(item);
                    break;
                case ItemType.MODULE:
                    Modules.Add(item);
                    break;
                default:
                    Resources.Add(item);
                    break;
            }
        }

        public List<ItemStack> GetInventory(List<Item> items, string filter)
        {
            List<ItemStack> inventory = items.MakeStacks();

            filter ??= "";

            inventory = (from ItemStack stack in inventory
                         where stack.item.Name.ToLower().Contains(filter.ToLower())
                         orderby (int)stack.item.data.rarity descending
                         select stack).ToList();

            return inventory;

        }
        
        
    }
}
