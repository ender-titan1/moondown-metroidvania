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
using Moondown.Sys.Template;
using UnityEngine;

namespace Moondown.Sys.Mono
{
    public class GroundedUnitEnemy : MonoBehaviour
    {
        public Unit Unit { get; protected set; }

        public static GroundedUnitEnemy New(Unit unit, Vector2 pos)
        {
            GroundedUnitEnemy gue = Instantiate(unit.template.prefab).GetComponent<GroundedUnitEnemy>();

            gue.Unit = unit;
            gue.gameObject.name = unit.template.name;
            gue.transform.position = pos;
            return gue;
        }
    } 
}