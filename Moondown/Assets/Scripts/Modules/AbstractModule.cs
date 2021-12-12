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

public abstract class AbstractModule
{
    public void setup()
    {
        PlayerManager.Instance.OnCharge += amount => OnCharge(amount);
        PlayerManager.Instance.OnHeal += amount => OnHeal(amount);
        PlayerManager.Instance.OnDamageTaken += amount => OnDamageTaken(amount);
        PlayerManager.Instance.OnRespawn += () => OnRespawn();
        PlayerManager.Instance.OnDeath += () => OnDeath();
    }

    public abstract void OnDamageTaken(int amount);
    public abstract void OnHeal(int amount);
    public abstract void OnCharge(int amount);
    public abstract void OnRespawn();
    public abstract void OnDeath();
}
