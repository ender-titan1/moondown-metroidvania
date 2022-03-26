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
using UnityEditor;
using UnityEngine;

namespace Moondown.AI.Enemy
{

    public class VisionCone : MonoBehaviour
    {
        private VisualSensor sensor;
        private Unit unit;

        private void OnEnable()
        {
            sensor = GetComponentInParent<VisualSensor>();
            unit = GetComponentInParent<VisualSensor>().GetComponentInParent<Unit>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                unit.CheckIfSpotted(sensor.Search(collision.gameObject));   
            }
        }
    }
}