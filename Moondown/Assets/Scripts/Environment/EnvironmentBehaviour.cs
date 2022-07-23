/*
    A script used to provide information about a GameObject
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
using UnityEngine;

namespace Moondown.Environment
{
    public class EnvironmentBehaviour : MonoBehaviour
    {
        [Header("Modifiers")]
        public int healthModifier = 0;
        public int chargeModifier = 0;
        public bool singleUse;

        [Space]
        public bool reset;

        [Header("Attack system")]
        public int healthPoints;
        public bool attackable;

        [HideInInspector]
        public bool isUsable;

        private void Start()
        {
            StartCoroutine(Refresh());
        }

        private void Update()
        {
            if (!attackable)
                return;

            if (healthPoints <= 0)
                Destroy(gameObject);
        }

        private IEnumerator Refresh()
        {
            yield return new WaitForSeconds(1);

            isUsable = true;
        }
    }
}