/*
    A script to manage the player's progress
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

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerManager : MonoBehaviour
{
    // singelton
    public static PlayerManager Instance { get; } = new PlayerManager();

    public delegate void actionDelegate(int amount);

    public event actionDelegate OnDamageTaken;
    public event actionDelegate OnHeal;
    public event actionDelegate OnCharge;

    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public int Charge { get; set; }
    public int MaxCharge { get; set; }

    public List<AbstractModule> modules = new List<AbstractModule> { };
    
    private void Update()
    {
        EnvironmentInteraction.Modifiers modifiers = EnvironmentInteraction.Instance.CheckCollisions();

        if (modifiers.charge > 0)
            OnCharge(modifiers.charge);

        if (modifiers.health > 0)
            OnHeal(modifiers.health);
        else
            OnDamageTaken(modifiers.health);
    }


}
