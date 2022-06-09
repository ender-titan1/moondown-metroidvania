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
using Moondown.Utility;
using UnityEngine;

namespace Moondown.Sys
{
    public class Unit
    {
        public enum Focus
        {
            Balanced = 0,
            MeleeFocused = 1,
            RangedFocused = -1,
        }

        public Focus focus;
        public int size;

        [UnitField]
        public int meleePower;
        [UnitField]
        public int rangedPower;

        public Unit(Focus f, int size)
        {
            meleePower = Random.Range(10, 101) + 15 * (int)f;
            rangedPower = Random.Range(10, 101) - 15 * (int)f;

            focus = f;
            this.size = size;
        }

        public Unit(int size) : this(Util.EnumRandom<Focus>(), size) { }

        public override string ToString()
        {
            return $"[\nsize: {size},\nfocus: {(int)focus},\nmelee: {meleePower},\nranged: {rangedPower}\n]";
        }
    }
}
