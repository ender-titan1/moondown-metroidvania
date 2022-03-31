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
    using Moondown.Player;

    public class Unit : MonoBehaviour, IEngagable
    {
        protected UnitState state = new UnitState.Idle();

        private Controller controller;
        private ControllerGroup group;
        private IEngagable target;
        protected Facing facing = Facing.Left;

        public float patrolLeft;
        public float patrolRight;

        [SerializeField] protected Vector2 zoneLeft;
        [SerializeField] protected Vector2 zoneRight;
        [SerializeField] protected BoxCollider2D zone;
        [SerializeField] protected EnemyData data;
 
        private float playerFound = 0;

        public Facing Facing => facing;
        public Controller Controller => controller;
        public ControllerGroup Group => group;

        public float MeleeStrength => data.meleeStrength;
        public float RangedStrength => data.rangedStrength;

        public IEngagable Target => target;

        protected Vector2 originalSize;

        public Vector2 playerPos;

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

        protected void Update()
        {
            state.Execute(this);
        }

        public virtual void Move(float target) { }

        public void CheckIfSpotted(SensorResult result)
        {
            if (result.found)
                playerFound += result.amount;


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

            playerPos = Player.Instance.GetGameObject().transform.position;

            Controller controller = new Controller(group);

            foreach (Unit unit in group.units)
            {
                unit.SetController(controller);
                state = new UnitState.Engaged();
            }

        }

        public void SetController(Controller controller) => this.controller = controller;

        public void SetTarget(IEngagable target) => this.target = target;

        public void SetState<T>() where T : UnitState, new() => state = new T();

        public GameObject GetGameObject() => gameObject;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject != gameObject)
                return;

            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.back, 10);
        }
#endif
    }
}