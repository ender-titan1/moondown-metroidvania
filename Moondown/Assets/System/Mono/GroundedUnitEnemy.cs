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
using Moondown.Sys.Mono.Zone;
using Moondown.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace Moondown.Sys.Mono
{
    public class GroundedUnitEnemy : MonoBehaviour
    {
        public enum EnemyTask
        {
            Stationary,
            Patrol,
            Search
        }

        public Unit Unit { get; protected set; }

        public EnemyTask Task { get; protected set; }

        public ControlZone Zone { get; protected set; }

        protected float endRadius;
        protected float endRadiusMin, endRadiusMax;
        protected float vel, speed;
        protected Facing facing;

        protected Path<Vector2> patrolPath;

        public static GroundedUnitEnemy New(Unit unit, Vector2 pos, ControlZone zone, Path<Transform> path)
        {
            GroundedUnitEnemy gue = Instantiate(unit.template.prefab).GetComponent<GroundedUnitEnemy>();

            gue.Unit = unit;
            gue.Zone = zone;
            gue.gameObject.name = unit.template.name;
            gue.transform.position = pos;
            gue.patrolPath = new Path<Vector2> { a = path.a.position, b = path.b.position }; 

            float size = gue.gameObject.transform.localScale.magnitude;

            gue.endRadiusMin = size * 0.75f;
            gue.endRadiusMax = size;
            gue.endRadius = unit.attention / 120 + UnityEngine.Random.Range(gue.endRadiusMin + 0.3f, gue.endRadiusMax - 0.3f);

            gue.vel = 3;
            gue.facing = Facing.Right;

            if (unit.template.capabilities.HasFlag(UnitCapability.Patrol))
                gue.Task = EnemyTask.Patrol;

            return gue;
        }

        public void OnDrawGizmosSelected()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.back, endRadiusMin);
            Handles.DrawWireDisc(transform.position, Vector3.back, endRadiusMax);

            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, Vector3.back, endRadius);

            Handles.color = Color.cyan;
            Handles.DrawWireDisc(patrolPath.a, Vector3.back, 0.5f);
            Handles.DrawWireDisc(patrolPath.b, Vector3.back, 0.5f);
            Handles.DrawLine(patrolPath.a, patrolPath.b);
        }

        public void FixedUpdate()
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

            bool canMove = !ScanEnd();

            Debug.Log(canMove);

            if (canMove)
            {
                rigidbody.velocity = new Vector2(vel * (int)facing, 0);
            }
            else
            {
                facing = facing.Reverse();
            }
        }

        public bool ScanEnd()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, endRadius, Vector3.forward);

            if (hits.Length == 0)
                return false;

            return Array.Exists(hits, hit =>
                {
                    return (Vector2)hit.transform.position == 
                                    (facing == Facing.Left ? patrolPath.a : patrolPath.b) && 
                                    hit.collider.CompareTag("Waypoint");
                }
            );
        }
    } 
}