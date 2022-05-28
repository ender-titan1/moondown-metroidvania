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
using System.Collections.Generic;
using UnityEngine;
using Moondown.Inventory;
using Moondown.Environment;
using Moondown.UI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moondown.Player.Movement;
using Moondown.UI.Inventory;
using Moondown.WeaponSystem.Attacks;

namespace Moondown.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Player : MonoBehaviour
    {
        // singelton
        public static Player Instance { get; private set; }

        public RespawnLocation LocalRespawn { get; set; }
        public RespawnLocation DeathRespawn { get; set; }

        public int health;
        public int MaxHealth { get; set; } = 5;

        public int charge;
        public int MaxCharge { get; set; } = 3;

        [SerializeField] private Material shaderMaterial;

        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members")]
        private MeeleAttack attack;

        [SerializeField] private LayerMask mask;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            attack = new MeeleAttack(GetComponent<BoxCollider2D>(), transform, mask);
        }

        private void Start()
        {
            DisplayHUD.Init(GameObject.FindGameObjectWithTag("hp bar"), GameObject.FindGameObjectWithTag("charge bar"));

            InventoryManager.Instance.Add(new Weapon("Weapon"));
            InventoryManager.Instance.Add(new Item("Scrap"), 100);
            InventoryManager.Instance.Add(new Item("RareItem"));
            InventoryManager.Instance.Add(new Item("Special"));
        }

        private void Update()
        {
            HandleEnvironmentInteraction();

            if (health <= 0)
                Die();

            DisplayHUD.UpdateCharge(charge);
            DisplayHUD.UpdateHealth(health);

        }

        // TODO: Redo respawn system
        private void HandleEnvironmentInteraction()
        {
           
        }

        private void Die()
        {
            health = MaxHealth;   
        }

        public async Task AnimateHazardFade()
        {
            PlayerMovement.Instance.controls.Disable();
            await Animate();
            
            Task delay = Task.Delay(2000);

            await delay;
            PlayerMovement.Instance.controls.Enable();

        }

        public async Task AnimateReverse()
        {
            float i = 1;
            while (i > 0)
            {
                shaderMaterial.SetFloat("_FadeValue", i);
                await Task.Delay(10);
                i -= 0.01f;
            }
        }

        private async Task Animate()
        {
            float i = 0;
            while (i < 1)
            {
                shaderMaterial.SetFloat("_FadeValue", i);
                await Task.Delay(10);
                i += 0.01f;
            }
        }

        public void EquipWeapon()
        {
            InventoryManager.Instance.Equip(DataPanel.Items.item);
            UIManager.Instance.InventoryUI.GetComponentInChildren<DisplayInventory>().RefreshEquipment();
        }
    }
}