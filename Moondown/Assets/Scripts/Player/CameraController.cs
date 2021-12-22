using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Graphics {
    
    using Moondown.Player;
    
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Vector3 pos;
        [SerializeField] private float speed;

        private void Update()
        {
            cam = Camera.main;
            pos = new Vector3(Player.Instance.gameObject.transform.position.x, Player.Instance.gameObject.transform.position.y, -10);
        }

        private void FixedUpdate()
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pos, speed * Time.fixedDeltaTime);
        }
    } 
}
