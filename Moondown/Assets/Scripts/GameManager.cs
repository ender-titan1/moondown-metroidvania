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

using Moondown.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public static event Action Tick;
    
        public List<Controller> Controllers { get; set; } = new List<Controller>();

        public LayerMask maskAI;

        public float playerStrengthMultiplier = 2;

        private void Awake()
        {
            if (Instance == null)
                Instance  = this;


            InvokeRepeating(nameof(ControllerTick), 0, 0.5f);
        }
       
        private void ControllerTick()
        {
            Tick?.Invoke();
        }
    }
}
