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

using Moondown.AI.Enemy.Modules.Sensor;
using Moondown.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Moondown.AI.Enemy
{
    public class GroundedEnemy : Unit
    {
        public override void Move(float target)
        {
            float targetX = Mathf.Clamp(target, ZoneLeft.x, ZoneRight.x);
            float movementAxis = (targetX - transform.position.x).ToAxis(0);

            facing = (Facing)movementAxis;
            transform.transform.localScale = (int)facing * -1 * originalSize;

            GetComponent<Rigidbody2D>().velocity = new Vector2(
                 movementAxis * GetSpeed(),
                0
            );
        }

        private float GetSpeed()
        {
            return state.GetType().Name switch 
            { 
                "Engaged" => data.speed * 1.5f,
                "Searching" => data.speed / 2,
                _ => data.speed
            };

        }

        private new void Update()
        {
            CheckIfSpotted(GetComponentInChildren<VisualSensor>().Search());

            base.Update();
        }
    }
}