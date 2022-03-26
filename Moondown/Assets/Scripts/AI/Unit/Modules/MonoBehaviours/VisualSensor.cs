using System.Collections;
using UnityEngine;

namespace Moondown.AI.Enemy.Modules.Sensor
{
    public class VisualSensor : MonoBehaviour, ISensor
    {
        public SensorResult Search(GameObject collision)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, 
                collision.transform.position - transform.position, 
                10, 
                layerMask: GameManager.Instance.maskAI
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