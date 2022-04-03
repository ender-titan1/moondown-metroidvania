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
using UnityEditor;
using UnityEngine;

namespace Moondown.AI.Enemy.Modules.Attack
{
    public class MeleeAttack : MonoBehaviour
    {
        public Unit unit;

        public int probeRadius;

        public int attackRange;
        public int damage;

        private void Awake()
        {
            unit = GetComponentInParent<Unit>();
        }

        public void Attack()
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                transform.position,
                Vector2.one * attackRange,
                0,
                unit.Facing == Facing.Left ? Vector2.left : Vector2.right
            );

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit!");
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (GetComponentInParent<Unit>().Controller == null)
                return;

            Handles.color = Color.red;
            Handles.DrawWireDisc(GetComponentInParent<Unit>().transform.position, Vector3.back, probeRadius);

            Vector2 pos = ((Vector2)transform.position) + (int)GetComponentInParent<Unit>().Facing * new Vector2(attackRange, (float)attackRange / 2);
            Rect rect = new Rect(pos, Vector2.one * attackRange);

            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.red);
        }

#endif

    }
}