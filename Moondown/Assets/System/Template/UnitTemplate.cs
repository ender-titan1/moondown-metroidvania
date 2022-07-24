﻿/*
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

namespace Moondown.Sys.Template
{
    [CreateAssetMenu(fileName = nameof(UnitTemplate), menuName = "Unit/Unit Template", order = 100)]
    public class UnitTemplate : ScriptableObject
    {
        public new string name;
        public GameObject prefab;

        public int rangedPreferance;
        public int meleePreferance;
        public int attentionPreferance;
        
        public int size;
        public UnitCapability capabilities;
        public bool unique;
    }
}