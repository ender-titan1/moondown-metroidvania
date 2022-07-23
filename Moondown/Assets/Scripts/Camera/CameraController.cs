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
using UnityEngine;

namespace Moondown.Graphics {
    
    using Moondown.Player;
    
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private float speed;

        private void Update()
        {
            pos = new Vector3(Player.Instance.gameObject.transform.position.x, Player.Instance.gameObject.transform.position.y, -10);
        }

        private void FixedUpdate()
        {
            float dist = Vector2.Distance(transform.position, pos);
            float moveSpeed = speed * Time.fixedDeltaTime * dist / 1.5f;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pos, moveSpeed);
        }
    } 
}