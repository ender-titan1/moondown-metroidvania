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

        public List<Item> Resources = new List<Item>();
        public List<Item> Weapons   = new List<Item>();
        public List<Item> Armour    = new List<Item>();
        public List<Item> Tools     = new List<Item>();
        public List<Item> Modules   = new List<Item>();
        
        public void Add(Item item)
        {
            if (item.data.type == ItemType.MEELE_WEAPON || item.data.type == ItemType.RANGED_WEAPON)
                Weapons.Add(item);
            else if (item.data.type == ItemType.TOOL)
                Tools.Add(item);
            else if (item.data.type == ItemType.ARMOUR)
                Armour.Add(item);
            else if (item.data.type == ItemType.MODULE)
                Modules.Add(item);
            else
                Resources.Add(item);
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
