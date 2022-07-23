namespace Moondown.Inventory
{
    public class Weapon : Item
    {
        public int Range { get; set; }
        public int Damage { get; set; }
        public Weapon(string n) : base(n)
        {
            Range = ((WeaponData)data).range;
            Damage = ((WeaponData)data).damage;
        }
    }
}
