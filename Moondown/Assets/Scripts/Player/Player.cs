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
using UnityEngine;
using Moondown.Inventory;
using Moondown.UI;
using Moondown.Utility;
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

        private Config config;

        private int _health, _charge;

        public int Health 
        {
            get => _health;

            set
            {
                _health = Mathf.Min(value, MaxHealth);
            }
        }

        public int MaxHealth { get; set; } = 5;

        public int Charge
        {
            get => _charge;

            set
            {
                _charge = Mathf.Min(value, MaxCharge);
            }
        }

        public int MaxCharge { get; set; } = 3;

        [SerializeField] private Material shaderMaterial;

        private MeeleAttack attack;

        [SerializeField] private LayerMask mask;

        private Vector2 hazardRespawnPoint = new Vector2(0, 0);
        private Vector2 deathRespawnPoint = new Vector2(0, 0);

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            attack = new MeeleAttack(GetComponent<BoxCollider2D>(), transform, mask);
            config = Config.Load();
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

            if (Health <= 0)
                Respawn();

            DisplayHUD.UpdateCharge(Charge);
            DisplayHUD.UpdateHealth(Health);

        }

        private void HandleEnvironmentInteraction()
        {
            InteractionResult res = EnvironmentInteraction.Result;

            if (config != null && !config.invincible)
                Health += res.health;

            Charge += res.charge;

            if (res.hazardRespawn.HasValue)
                hazardRespawnPoint = (Vector2)res.hazardRespawn;

            if (res.deathRespawn.HasValue)
                deathRespawnPoint = (Vector2)res.deathRespawn;

            if (Health <= 0)
            {
                Respawn();
            }
            else if (res.hasBeenHit)
            {
                PlayerMovement.Instance.CancelInvoke("CancelDash");
                PlayerMovement.Instance.CancelDash();
                transform.position = hazardRespawnPoint;
            }

            foreach (GameObject go in res.toDestroy)
                Destroy(go);
        }

        private void Respawn()
        {
            PlayerMovement.Instance.CancelInvoke("CancelDash");
            PlayerMovement.Instance.CancelDash();
            transform.position = deathRespawnPoint;
            Health = MaxHealth;   
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