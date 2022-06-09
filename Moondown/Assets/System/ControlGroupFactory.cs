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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Moondown.Utility;
using System.Linq;

namespace Moondown.Sys
{
    class ControlGroupFactory
    {
        public ControlGroup MakeGroup(int size)
        {
            // Generate units

            ControlGroup.Task task = Util.EnumRandom<ControlGroup.Task>();
            int unitSize = 0;

            List<Unit> units = new List<Unit>();

            while (true)
            {
                Unit unit = new Unit(UnityEngine.Random.Range(1, 5));

                if (unitSize + unit.size <= size)
                {
                    unitSize += unit.size;
                    units.Add(unit);
                }
                else
                {
                    break;
                }
            }

            ControlGroup cg = new ControlGroup(units.ToArray(), task);

            return cg;
        }
    }
}
