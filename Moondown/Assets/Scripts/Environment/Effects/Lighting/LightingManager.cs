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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Moondown.Environment.Effects
{
    public class LightingManager : MonoBehaviour
    {
        [Flags]
        public enum LightingModes
        {
            NONE = 0,
            BACKGROUND = 1,
            FOREGROUND = 2,
            MAIN = 4,
            PLAYER = 8,
            GROUND = 16
        }

        private GameObject[] GetLights(LightingModes mode)
        {
            List<GameObject> @out = new List<GameObject>();
            if (mode.HasFlag(LightingModes.BACKGROUND))
                @out.Add(GameObject.FindGameObjectWithTag("global light bg"));
            if (mode.HasFlag(LightingModes.PLAYER))
                @out.Add(GameObject.FindGameObjectWithTag("global light player"));
            if (mode.HasFlag(LightingModes.GROUND))
                @out.Add(GameObject.FindGameObjectWithTag("global light ground"));

            return @out.ToArray();
        }

        // TODO: Make it gradual
        public void SetIntensity(LightingModes mode, float value)
        {
            float val = Mathf.Clamp(value, 0, 1);
            GameObject[] lights = GetLights(mode);

            foreach (GameObject light in lights)
            {
                Light2D L2D = light.GetComponent<Light2D>();
                L2D.intensity = val;
            }
        }
    }
}