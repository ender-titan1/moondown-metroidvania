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

using Moondown.AI.Event;
using Moondown.Player.Movement;
using Moondown.Utility;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Moondown.AI
{
    public class Unit : MonoBehaviour
    {
        private bool engaged;
        private Controller controller;
        private Facing facing;
        private MoondownEvent currentEvent;
        private ControllerGroup group;
        private ITargetable target;

        [SerializeField] private Vector2 zoneLeft;
        [SerializeField] private Vector2 zoneRight;
        [SerializeField] private BoxCollider2D zone;
        [SerializeField] private float speed = 5;

        // should be put into a game manager later
        [SerializeField] private LayerMask mask;

        public Facing Facing => facing;
        public Controller Controller
        {
            get => controller;

            set => controller = value;
        }
        public ControllerGroup Group => group;

        private void OnEnable()
        {
            group = new ControllerGroup()
            {
                units = new List<Unit>() { this }
            };
        }

        private void Awake()
        {
            Bounds zoneBounds = zone.bounds;
            zoneRight = new Vector2(zoneBounds.center.x + zoneBounds.extents.x, zoneBounds.center.y + zoneBounds.extents.y);
            zoneLeft = new Vector2(zoneBounds.center.x - zoneBounds.extents.x, zoneBounds.center.y - zoneBounds.extents.y);
        }

        private void Update()
        {
            if (engaged)
                Pathfind();
            else
                Patrol();
        }

        private void Patrol()
        {
            // follow a patrol path
            // if the player enters a vision cone, engage
        }


        private void Pathfind()
        {
            if (CheckPlayer())
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                return;
            }

            // Later the movement sholud be handled by the enemy class
            // check if grounded is required
            float playerX = Mathf.Clamp(Player.Player.Instance.transform.position.x, zoneLeft.x, zoneRight.x);
            float movementAxis = (playerX - transform.position.x).ToAxis(0);

            facing = (Facing)movementAxis;

            GetComponent<Rigidbody2D>().velocity = new Vector2(
                 movementAxis * speed,
                0
            );

        }

        private bool CheckPlayer()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 10, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    RaycastHit2D check = Physics2D.Raycast(transform.position, hit.transform.position - transform.position, 10, layerMask: mask);

                    Debug.DrawLine(transform.position, hit.transform.position, Color.green);

                    if (check.collider == null)
                        return false;
                    
                    if (check.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player Found!");
                        return true;
                    }
                }

            }

            return false;
        }

        public void PlayerSpotted()
        {
            currentEvent = MoondownEvent.Of(this);   
        }

        public void SetTarget(ITargetable target)
        {
            this.target = target;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.back, 10);
        }
#endif
    }
}