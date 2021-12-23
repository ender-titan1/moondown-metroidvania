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
using UnityEngine.UI;

namespace Moondown.UI
{
    using Moondown.Player;

    public static class DisplayHUD 
    {
        private static GameObject[] healthBar;
        private static GameObject[] chargeBar;

        public static void Init(GameObject health, GameObject charge)
        {

            List<GameObject> healthBarList = new List<GameObject> { };
            List<GameObject> chargeBarList = new List<GameObject> { };

            foreach (Transform child in health.transform)
                healthBarList.Add(child.gameObject);

            foreach (Transform child in charge.transform)
                chargeBarList.Add(child.gameObject);

            healthBar = healthBarList.ToArray();
            chargeBar = chargeBarList.ToArray();
        }

        public static void UpdateHealth(int amount)
        {
            for (int i = 0; i < healthBar.Length; i++)
            {
                if (i < amount)
                    healthBar[i].GetComponent<RawImage>().color = new Color(0, 1, 0, 1);
                else
                    healthBar[i].GetComponent<RawImage>().color = new Color(0, 1, 0, 0.2705882f);
            }
        }

        public static void UpdateCharge(int amount)
        {
            for (int i = 0; i < chargeBar.Length; i++)
            {
                if (i < amount)
                    chargeBar[i].GetComponent<RawImage>().color = new Color(0, 1, 1, 1);
                else
                    chargeBar[i].GetComponent<RawImage>().color = new Color(0, 0, 1, 0.2705882f);
            }
        }
    }
}
