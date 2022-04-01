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
using UnityEngine;

namespace Moondown.AI.Enemy
{
    public abstract class UnitState
    {
        //TODO: make this a variable
        private const float DISTANCE = 10f;

        public abstract void Execute(Unit unit);

        public class Idle : UnitState
        {
            public override void Execute(Unit unit)
            {
                unit.Move(unit.Facing == Facing.Left ? unit.patrolLeft : unit.patrolRight);
            }
        }

        public class Engaged : UnitState
        {
            public override void Execute(Unit unit)
            {
                unit.Move(unit.Target.GetGameObject().transform.position.x);
            }
        }

        public class Searching : UnitState
        {
            public override void Execute(Unit unit)
            {
                // last sighted position
                float playerX = unit.playerPos.x;

                // variation
                float leftVariation = Random.Range(-5, 5);
                float rightVariation = Random.Range(-5, 5);

                // points
                float left = Mathf.Clamp(playerX - DISTANCE + leftVariation, unit.ZoneLeft.x, unit.ZoneRight.x);
                float right = Mathf.Clamp(playerX + DISTANCE + rightVariation, unit.ZoneLeft.x, unit.ZoneRight.x);

                unit.Move(unit.Facing == Facing.Left ? left : right);
            }
        }
    }
}