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

using Moondown.Utility;
using Moondown.AI.Enemy;
using Moondown.AI.Enemy.Modules.Sensor;
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
        private ControllerGroup group;
        private ITargetable target;
        protected Facing facing = Facing.Left;

        [SerializeField] private float patrolLeft;
        [SerializeField] private float patrolRight;

        [SerializeField] protected Vector2 zoneLeft;
        [SerializeField] protected Vector2 zoneRight;
        [SerializeField] protected BoxCollider2D zone;
        [SerializeField] protected float speed = 5;

        private float playerFound = 0;

        public Facing Facing => facing;
        public Controller Controller => controller;
        public ControllerGroup Group => group;

        protected Vector2 originalSize;

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

            originalSize = transform.localScale;
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
            Move(facing == Facing.Left ? patrolLeft : patrolRight);
        }


        private void Pathfind()
        {
            if (CheckTarget())
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                return;
            }

            Move(target.GetGameObject().transform.position.x);
        }

        protected virtual void Move(float target)
        {

        }

        private bool CheckTarget()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 10, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag(target.GetGameObject().tag))
                {
                    RaycastHit2D check = Physics2D.Raycast(transform.position, hit.transform.position - transform.position, 10, layerMask: GameManager.Instance.maskAI);

                    DrawDebugLine(hit);

                    if (check.collider == null)
                        return false;

                    if (check.collider.CompareTag(target.GetGameObject().tag))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        public void CheckIfSpotted(SensorResult result)
        {
            if (result.found)
            {
                playerFound += result.amount;
            }

            if (playerFound >= 1)
            {
                playerFound = 0;
                PlayerSpotted();
            }
        } 

        private void PlayerSpotted()
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