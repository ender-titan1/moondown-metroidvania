namespace Moondown.Inventory
{
    public struct Weapon
    {
        public int Range { get; set; }
        public int Damage { get; set; }

        private Item item;

        public Weapon(string n)
        {
            item = new Item(n);
            WeaponData data = ((WeaponData)item.data);
            Range = data.range;
            Damage = data.damage;
        }

        public static implicit operator Item(Weapon weapon) => weapon.item;
    }
}
