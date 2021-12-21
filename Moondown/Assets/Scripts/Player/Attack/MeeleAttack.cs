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
using Moondown.Inventory;
using Moondown.Player.Movement;
using Moondown.Environment;
using Moondown.Utility;

namespace Moondown.WeaponSystem.Attacks
{
    using Moondown.Player;
    
    public sealed class MeeleAttack
    {
        private readonly MainControls controls;
        private readonly BoxCollider2D collider;
        private readonly Transform transform;
        private readonly LayerMask mask;

        public MeeleAttack(BoxCollider2D collider, Transform transform, LayerMask mask)
        {
            controls = new MainControls();

            controls.Player.AttackMeele.performed += _ => Attack();
            controls.Enable();

            this.collider = collider;
            this.transform = transform;
            this.mask = mask;
        }

        private void Attack()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(
                new Vector2(
                    transform.position.x + EquipmentManager.Instance.EquipedWeapon.Range * (int)PlayerMovement.Instance.facing,
                    transform.position.y
                ),
                collider.size,
                0f,
                PlayerMovement.Instance.facing == Facing.LEFT ? Vector2.left : Vector2.right,
                collider.size.x,
                mask
            );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                    continue;

                if (hit.collider.Has<EnvironmentBehaviour>() && hit.collider.GetComponent<EnvironmentBehaviour>().attackable)
                {
                    EnvironmentBehaviour behaviour = hit.collider.GetComponent<EnvironmentBehaviour>();
                    behaviour.healthPoints -= EquipmentManager.Instance.EquipedWeapon.Damage;
                }
                else if (hit.collider.Has<RespawnLocation>() && hit.collider.GetComponent<RespawnLocation>().cost > 0)
                {
                    if (Player.Instance.DeathRespawn.cost == 0)
                        Object.Destroy(Player.Instance.DeathRespawn.gameObject);

                    hit.collider.GetComponent<RespawnLocation>().Activate();
                }
            }

        }


    }
}