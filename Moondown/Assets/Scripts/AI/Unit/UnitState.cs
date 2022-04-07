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

namespace Moondown.AI.Enemy
{
    public abstract class UnitState
    {
        //TODO: make this a variable
        private const float DISTANCE = 10f;

        public abstract void Execute();
        public abstract UnitState SetUnit(Unit unit);

        public virtual void DrawGizmos() { }

        public UnitState(Unit unit) { }

        /////////////////////////////////////////////

        public class Idle : UnitState
        {
            Unit unit;

            public Idle(Unit u) : base(u) => unit = u;

            public override void Execute()
            {
                unit.Move(unit.Facing == Facing.Left ? unit.patrolLeft : unit.patrolRight);
            }

            public override UnitState SetUnit(Unit unit)
            {
                this.unit = unit;
                return this;
            }
        }

        public class Engaged : UnitState
        {
            Unit unit;

            public Engaged(Unit u) : base(u) => unit = u;

            public override void Execute()
            {
                unit.Move(unit.Target.GetGameObject().transform.position.x);
            }

            public override UnitState SetUnit(Unit unit)
            {
                this.unit = unit;
                return this;
            }
        }

        public class Searching : UnitState
        {
            float left, right;
            Unit unit;

            #region Creation
            public Searching(Unit unit) : base(unit)
            {
                this.unit = unit;

                if (unit == null)
                    return;

                Construct(unit);
            }

            private void Construct(Unit unit)
            {
                // last sighted position
                float playerX = unit.playerPos.x;

                // variation
                float leftVariation = Random.Range(-5, 5);
                float rightVariation = Random.Range(-5, 5);

                // points
                left = Mathf.Clamp(playerX - DISTANCE + leftVariation, unit.ZoneLeft.x, unit.ZoneRight.x);
                right = Mathf.Clamp(playerX + DISTANCE + rightVariation, unit.ZoneLeft.x, unit.ZoneRight.x);
            }

            public override UnitState SetUnit(Unit unit)
            {
                this.unit = unit;
                Construct(unit);
                return this;
            }
            #endregion

            public override void Execute()
            {
                unit.Move(unit.Facing == Facing.Left ? left : right);
            }

            public override void DrawGizmos()
            {
                Handles.color = Color.yellow;

                float unitY = unit.gameObject.transform.position.y;

                Handles.DrawWireDisc(new Vector2(left, unitY), Vector3.back, 0.5f);
                Handles.DrawWireDisc(new Vector2(right, unitY), Vector3.back, 0.5f);

                Handles.color = new Color(1, 0.92f, 0.016f, 0.75f);

                Handles.DrawLine(new Vector2(left, unitY), new Vector2(right, unitY));
            }
        }

    }
}