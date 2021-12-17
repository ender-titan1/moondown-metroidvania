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
    public static PlayerManager Instance { get; private set; }

    public delegate void ActionDelegate(int amount);
    public delegate void BlankDelegate();

    public event ActionDelegate OnDamageTaken;
    public event ActionDelegate OnHeal;
    public event ActionDelegate OnCharge;
    public event BlankDelegate OnDeath;
    public event BlankDelegate OnRespawn;

    public RespawnLocation LocalRespawn { get; set; }
    public RespawnLocation DeathRespawn { get; set; }

    public int Health { get; set; }
    public int MaxHealth { get; set; } = 10;

    public int Charge { get; set; }
    public int MaxCharge { get; set; } = 3;

    public List<AbstractModule> modules = new List<AbstractModule> { };

    [SerializeField] private Sprite baseSprite;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        modules.Add(new BasePlayerModule());

        Weapon weapon = new Weapon(
            LocalizationManager.Get("BASIC_SWORD_NAME"),
            LocalizationManager.Get("BASIC_SWORD_DESC"),
            "UI/Inventory/Placeholder",
            1,
            AttackMode.NORMAL,
            ItemType.MEELE_WEAPON,
            baseSprite
        );

        EquipmentManager.Instance.Inventory.Add(weapon);

        OnRespawn();
    }

    private void Update()
    {
        EnvironmentInteraction.Modifiers modifiers = EnvironmentInteraction.Instance.CheckCollisions();

        if (modifiers.charge > 0)
            OnCharge(modifiers.charge);

        if (modifiers.health != 0)
        {

            if (modifiers.health > 0)
                OnHeal(modifiers.health);
            else
                OnDamageTaken(modifiers.health);
        }

        if (modifiers.hasBeenHit)
        {
            gameObject.transform.position = LocalRespawn.position;
        }

        if (Health <= 0)
            Die();

    }

    private void Die()
    {
        OnDeath();
        OnRespawn();
    }


}
