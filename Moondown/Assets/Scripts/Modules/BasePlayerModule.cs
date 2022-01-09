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
using Moondown.Environment.Effects;
using static Moondown.Environment.Effects.LightingManager;
using System.Threading.Tasks;

namespace Moondown.Player.Modules
{
    public class BasePlayerModule : GraphicsModule
    {
        public BasePlayerModule() => setup();

        public override void OnHeal(int amount)
        {
            Player.Instance.health += amount;
        }

        public override void OnCharge(int amount)
        {
            Player.Instance.charge += amount;
        }

        public override void OnDamageTaken(int amount)
        {
            // this is done because the 'amount' parameter is negative
            Player.Instance.health += amount;
        }

        public override void OnRespawn()
        {
            Player.Instance.health += Player.Instance.MaxHealth;
        }

        public override void OnDeath()
        {
            Player.Instance.gameObject.transform.position = Player.Instance.DeathRespawn.position;
        }

        public override void OnApplyLowHealth()
        {
            Player.Instance.LowHealthPostProcessing.SetActive(true);
        }

        public override void OnClearEffects()
        {
            Player.Instance.LowHealthPostProcessing.SetActive(false);
        }

        public override void OnHazardRespawn()
        {
            LightingManager lighting = GameObject.FindGameObjectWithTag("manager effects").GetComponent<LightingManager>();
            LightingModes target = (LightingModes.BACKGROUND | LightingModes.GROUND);

            lighting.SetIntensity(target, 0);
        }
    }
}