namespace Moondown.Inventory
{
    public struct ItemStack
    {
        public Item item;
        public int amount;

        public ItemStack(Item item, int amount)
        {
            this.amount = amount;
            this.item = item;
        }
    }
}
