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
using System.Collections;
using UnityEngine;
using System;
using UnityEditor;

namespace Moondown.AI.Enemy.Modules.Sensor
{
    using Moondown.Player;

    public class VisualSensor : MonoBehaviour, ISensor
    {
        private Unit unit;

        [SerializeField] private Cone cone;

        private float originalRange;

        private void Awake()
        {
            unit = GetComponentInParent<Unit>();

            cone.origin = transform.position;

            GameObject area = new GameObject($"{name}Area");
            area.transform.SetParent(transform);

            PolygonCollider2D collider = area.AddComponent<PolygonCollider2D>();

            Vector2[] points = new Vector2[3]
            {
                cone.origin,
                cone.Sample1,
                cone.Sample2
            };

            collider.pathCount = 1;
            collider.SetPath(0, points);
            collider.isTrigger = true;

            originalRange = cone.range;
        }

        private void Update()
        {
            cone.origin = transform.position;

            cone.range = ((int)unit.Facing) * originalRange;
        }

        public SensorResult Search()
        {
            Vector2 toPlayer = Player.Instance.GetGameObject().transform.position - transform.position;

            if (gameObject.GetChildren().Length == 0)
                return SensorResult.failed;

            if (!cone.In((c) => c.gameObject == Player.Instance.GetGameObject(), gameObject.GetChildren()[0]))
                return SensorResult.failed;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, 
                toPlayer, 
                10, 
                GameManager.Instance.maskAI
            );

            return new SensorResult(
                hit.collider != null && hit.collider.CompareTag("Player"), 
                1
            );
        }
    }
}