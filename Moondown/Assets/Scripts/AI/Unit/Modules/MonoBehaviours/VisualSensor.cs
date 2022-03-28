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
        [SerializeField] private bool debugMode;

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
            Search();

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

        public void Toggle(bool value)
        {
            gameObject.SetActive(value);
        }


#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (GetComponentInParent<Unit>().gameObject != Selection.activeGameObject && gameObject != Selection.activeGameObject && !debugMode)
                return;

            Cone visibleCone = new Cone()
            {
                origin = transform.position,
                size = cone.size,
                range = cone.range 
            };

            Handles.color = Color.cyan;
            Handles.DrawPolyLine(visibleCone.origin, visibleCone.Sample1, visibleCone.Sample2, visibleCone.origin);

            Handles.color = new Color(0, 1, 1, 0.25f);
            Handles.DrawLine(visibleCone.origin, visibleCone.SampleCenter);
        }

#endif
    }
}