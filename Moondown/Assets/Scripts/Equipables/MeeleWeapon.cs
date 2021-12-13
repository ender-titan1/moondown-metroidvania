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
