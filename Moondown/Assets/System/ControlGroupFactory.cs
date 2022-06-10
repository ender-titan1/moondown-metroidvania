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

            int unitSize = 0;

            List<Unit> units = new List<Unit>();
            List<FieldInfo> maxFields = new List<FieldInfo>();
            List<FieldInfo> minFields = new List<FieldInfo>();

            while (true)
            {
                Unit unit = new Unit(UnityEngine.Random.Range(1, 4));

                if (unitSize + unit.size <= size)
                {
                    unitSize += unit.size;
                    units.Add(unit);

                    (FieldInfo, FieldInfo) minMax = unit.GetMinMax();
                    minFields.Add(minMax.Item1);
                    maxFields.Add(minMax.Item2);
                }
                else
                {
                    break;
                }
            }

            // Get the most common lowest and highest value fields

            FieldInfo minField = (from FieldInfo fi in minFields
                                  group fi by fi into fig
                                  orderby fig.Count() descending
                                  select fig.Key).First();

            FieldInfo maxField = (from FieldInfo fi in maxFields
                                  group fi by fi into fig
                                  orderby fig.Count() descending
                                  select fig.Key).First();

            // Amplify each unit's lowest and highest values

            foreach (Unit unit in units)
            {
                minField.SetValue(
                    unit, 
                    Mathf.Clamp(
                        (int)minField.GetValue(unit) - UnityEngine.Random.Range(10, 50), 
                        20, 
                        130
                    )
                );


                maxField.SetValue(
                    unit,
                    Mathf.Clamp(
                        (int)maxField.GetValue(unit) + UnityEngine.Random.Range(10, 50),
                        20,
                        130
                    )
                );
            }

            // Generate Counter Unit

            Unit counter = new Unit(2);
            minField.SetValue(
                    counter,
                    Mathf.Clamp(
                        (int)minField.GetValue(counter) + UnityEngine.Random.Range(10, 60),
                        20,
                        130
                    )
                );


            maxField.SetValue(
                counter,
                Mathf.Clamp(
                    (int)maxField.GetValue(counter) - UnityEngine.Random.Range(10, 60),
                    20,
                    130
                )
            );

            units.Add(counter);

            // Display Values

            Debug.Log(minField);
            Debug.Log(maxField);

            // Create Group

            ControlGroup.Task task = Util.EnumRandom<ControlGroup.Task>();
            ControlGroup cg = new ControlGroup(units.ToArray(), task);

            return cg;
        }
    }
}
