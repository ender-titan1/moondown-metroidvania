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
using System.Linq;
using UnityEngine;

namespace Moondown.Player
{

    [System.Serializable]
    public struct InteractionResult
    {
        public int health;
        public int charge;

        public bool hasBeenHit;
        public bool climbable;

        public Vector3? hazardRespawn;
        public Vector3? deathRespawn;

        public GameObject[] toDestroy;

        public static InteractionResult operator +(InteractionResult a, InteractionResult b)
        {
            return new InteractionResult
            {
                health = a.health + b.health,
                charge = a.charge + b.charge,
                hasBeenHit = a.hasBeenHit || b.hasBeenHit,
                climbable = a.climbable || b.climbable,
                hazardRespawn = b.hazardRespawn ?? a.hazardRespawn,
                deathRespawn = b.deathRespawn ?? a.deathRespawn,
                toDestroy = a.toDestroy.Concat(b.toDestroy).ToArray()
            };
        }
    }

}