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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Environment
{
    using Moondown.Player;

    public class RespawnLocation : MonoBehaviour
    {
        public enum RespawnMode
        {
            Hazard,
            Death
        }

        public Vector2 position;
        public RespawnMode mode;
        public int cost;

        private void OnEnable()
        {
            position = gameObject.transform.position;
        }

        public void Activate()
        {
            if (Player.Instance.charge >= cost)
            {
                Player.Instance.charge -= cost;
                Player.Instance.DeathRespawn = this;
            }
        }
    }
}