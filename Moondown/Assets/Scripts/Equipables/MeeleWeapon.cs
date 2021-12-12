using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeapon : AbstractEquipable
{

    public override string Name { get; protected set; }
    public override string Description { get; protected set; } 
    public override AbstractModule Module { get; set; }


    public int Damage { get; private set; }
    public AttackMode Mode { get; private set; }

    public MeeleWeapon(string name, string desc, AbstractModule module, int dmg, AttackMode mode)
    {
        this.Name = name;
        this.Description = desc;
        this.Module = module;
        this.Damage = dmg;
        this.Mode = mode;
    }


}
