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
using Moondown.Environment;
using Moondown.Player.Movement;

namespace Moondown.Player
{
    // TODO: Rework this file?
    public class EnvironmentInteraction : MonoBehaviour
    {
        // TODO: Extract to own file
        public struct Result
        {
            public int health;
            public int charge;

            public bool hasBeenHit;
            public bool climbable;
        }

        public static EnvironmentInteraction Instance { get; private set; }

        private List<GameObject> gameObjects = new List<GameObject> { };
        public MainControls controls;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            controls = new MainControls();

            controls.Player.Interact.performed += _ =>
            {
                Debug.Log("hi");
                HandleInteract();
            };
        }

        #region Handle Collisions

        public Result CheckCollisions()
        {
            Result m = new Result();
            m.charge = 0;
            m.health = 0;

            foreach (GameObject collision in gameObjects)
            {
                EnvironmentBehaviour behaviour = collision.GetComponent<EnvironmentBehaviour>();

                m.charge += behaviour.chargeModifier;
                m.health += behaviour.healthModifier;

                if (behaviour.reset)
                    m.hasBeenHit = true;

                if (behaviour.singleUse)
                    Destroy(behaviour.gameObject);

                behaviour.isUsable = false;
            }

            gameObjects.Clear();
            return m;

        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("climbable"))
                gameObjects.Add(collision.gameObject);

            if (collision.gameObject.GetComponent<EnvironmentBehaviour>() != null)
                gameObjects.Add(collision.gameObject);

            RespawnLocation respawn = collision.gameObject.GetComponent<RespawnLocation>();

            if (respawn != null)
            {
                if (respawn.mode == RespawnLocation.RespawnMode.HAZARD)
                    Player.Instance.LocalRespawn = respawn;
                else
                {
                    if (respawn.cost == 0)
                        Player.Instance.DeathRespawn = respawn;
                }
            }
        }

        #endregion

        private void HandleInteract()
        {
            Result res = CheckCollisions();

            Debug.Log(res.climbable);
            if (res.climbable)
            {
                PlayerMovement playerMovement = Player.Instance.GetComponent<PlayerMovement>();

                if (playerMovement.mode == PlayerMovement.Mode.Normal)
                    playerMovement.mode = PlayerMovement.Mode.Climbing;
                else if (playerMovement.mode == PlayerMovement.Mode.Climbing)
                    playerMovement.mode = PlayerMovement.Mode.Normal;
            }
        }
    }
}