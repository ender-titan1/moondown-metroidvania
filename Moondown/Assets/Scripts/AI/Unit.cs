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
        private ControllerGroup group;
        private ITargetable target;

        [SerializeField] private Vector2 zoneLeft;
        [SerializeField] private Vector2 zoneRight;
        [SerializeField] private BoxCollider2D zone;
        [SerializeField] private float speed = 5;

        // should be put into a game manager later
        [SerializeField] private LayerMask mask;

        public Facing Facing => facing;
        public Controller Controller => controller;
        public ControllerGroup Group => group;

        private void OnEnable()
        {
            group = new ControllerGroup()
            {
                units = new List<Unit>() { this },
                name  = zone.name
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
            float targetX = Mathf.Clamp(target.GetGameObject().transform.position.x, zoneLeft.x, zoneRight.x);
            float movementAxis = (targetX - transform.position.x).ToAxis(0);

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
                if (hit.collider.CompareTag(target.GetGameObject().tag))
                {
                    RaycastHit2D check = Physics2D.Raycast(transform.position, hit.transform.position - transform.position, 10, layerMask: mask);

                    DrawDebugLine(hit);

                    if (check.collider == null)
                        return false;

                    if (check.collider.CompareTag(target.GetGameObject().tag))
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
            if (this.controller != null)
                return;

            Controller controller = new Controller(group);

            foreach (Unit unit in group.units)
            {
                unit.SetController(controller);
                unit.engaged = true;
            }

        }

        public void SetController(Controller controller)
        {
            this.controller = controller;
        }

        public void SetTarget(ITargetable target)
        {
            this.target = target;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject != gameObject)
                return;

            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.back, 10);
        }

        private void DrawDebugLine(RaycastHit2D hit)
        {
            if (Selection.activeGameObject != gameObject)
                return;

            Debug.DrawLine(transform.position, hit.transform.position, Color.green);
        }
#else
        private void DrawDebugLine(RaycastHit2D hit)
        {
        }
#endif
    }
}