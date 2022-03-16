using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
