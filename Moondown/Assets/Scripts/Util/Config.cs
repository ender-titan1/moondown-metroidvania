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
using System.IO;
using UnityEngine;

namespace Moondown.Utility
{
    [Serializable]
    class Config
    {
        public float playerGravity;
        public float playerJumpVelocity;
        public float playerDashVelocity;
        public float playerWallJumpVelocity;
        public float playerMass;
        public bool invincible;

        public static Config Load()
        {
            string path = Application.persistentDataPath + "/GlobalConfig.json";

            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                Config cfg = JsonUtility.FromJson<Config>(text);
                return cfg;
            }
            else
            {
                Debug.LogError("Configuration file not found!");
                return null;
            }
        }
    }
}
