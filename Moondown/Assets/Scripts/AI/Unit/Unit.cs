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
using Moondown.AI.Enemy.Modules;
using Moondown.AI.Enemy.Modules.Sensor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Moondown.AI
{
    using Moondown.Player;

    public class Unit : MonoBehaviour, IEngagable, ISensor
    {
        protected UnitState state;

        protected Vector2 originalSize;
        public Vector2 playerPos;

        [SerializeField] protected EnemyData data;
        
        #region AI
        private Controller controller;
        private ControllerGroup group;
        private IEngagable target;

        private float playerFound = 0;

        [SerializeField] private float intelRadius; 
        #endregion

        #region Movement
        protected Facing facing = Facing.Left;
        public float patrolLeft;
        public float patrolRight;

        public Vector2 ZoneLeft
        {
            get
            {
                Bounds zoneBounds = zone.bounds;
                return new Vector2(zoneBounds.center.x - zoneBounds.extents.x, zoneBounds.center.y - zoneBounds.extents.y);
            }
        }
        public Vector2 ZoneRight
        {
            get
            {
                Bounds zoneBounds = zone.bounds;
                return new Vector2(zoneBounds.center.x + zoneBounds.extents.x, zoneBounds.center.y + zoneBounds.extents.y);
            }
        }

        [SerializeField] protected BoxCollider2D zone;
        #endregion

        #region Properties
        public Facing Facing => facing;
        public Controller Controller => controller;
        public ControllerGroup Group => group;

        public float MeleeStrength => data.meleeStrength;
        public float RangedStrength => data.rangedStrength;

        public UnitState State => state;

        public IEngagable Target => target;
        #endregion

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
            state = new UnitState.Idle(this);

            originalSize = transform.localScale;
        }

        protected void Update()
        {
            state.Execute();
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
            playerPos = Player.Instance.GetGameObject().transform.position;
            
            if (this.controller != null)
                return;

            Controller controller = new Controller(group);

            foreach (Unit unit in group.units)
            {
                unit.SetController(controller);
            }

        }

        public void SetController(Controller controller) => this.controller = controller;

        public void SetTarget(IEngagable target) => this.target = target;

        public void SetState(UnitState state) => this.state = state;

        public GameObject GetGameObject() => gameObject;

        #region Sensor

        /// <summary>
        /// Intel search, 
        /// used to check if the unit should go into search mode
        /// </summary>
        /// <returns>The result of the search</returns>
        public SensorResult Search()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, intelRadius, Vector2.zero, GameManager.Instance.maskAI);

            foreach (RaycastHit2D h in hits)
            {
                if (h.collider.CompareTag("Player"))
                {

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.Instance.gameObject.transform.position - transform.position, intelRadius, GameManager.Instance.maskAI);

                    if (hit.collider.CompareTag("Player"))
                        return new SensorResult
                        {
                            found = true,
                            amount = 1
                        };
                }
            }

            return SensorResult.failed;
        }

        public void Toggle(bool value) { }

        #endregion

        #region Editor
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.activeGameObject != gameObject)
                return;

            Handles.color = Color.white;
            Handles.DrawWireDisc(transform.position, Vector3.back, intelRadius);

            if (Player.Instance == null)
                return;

            Handles.color = Color.gray;
            Handles.DrawLine(transform.position, Player.Instance.gameObject.transform.position);

            Handles.color = Color.cyan;
            Handles.DrawWireDisc(playerPos, Vector3.back, 1);

            state.DrawGizmos();
        }
#endif
        #endregion
    }
}