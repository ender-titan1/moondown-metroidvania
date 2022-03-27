using Moondown.Utility;
using System.Collections;
using UnityEngine;

namespace Moondown.AI.Enemy.Modules.Sensor
{
    using Moondown.Player;
    using System;
    using UnityEditor;

    public class VisualSensor : MonoBehaviour, ISensor
    {
        private Unit unit;

        [SerializeField] private Cone cone;

        private void Awake()
        {
            unit = GetComponentInParent<Unit>();

            cone.origin = transform.position;

            GameObject area = new GameObject($"{name} Area");
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
        }

        private void Update()
        {
            Search();

            cone.origin = transform.position;

            cone.range *= (int)unit.Facing;
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

        public void Toggle(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}