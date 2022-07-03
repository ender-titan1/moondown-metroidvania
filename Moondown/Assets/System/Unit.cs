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
using System.Reflection;
using System.Linq;
using Moondown.Utility;
using Moondown.Sys.Template;

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
        public UnitTemplate template;

        [UnitField]
        public int meleePower;
        [UnitField]
        public int rangedPower;
        [UnitField]
        public int attention;

        public Unit(Focus f, int size, UnitTemplate template)
        {
            meleePower =  UnityEngine.Random.Range(30, 121) + 20 * (int)f;
            rangedPower = UnityEngine.Random.Range(30, 121) - 20 * (int)f;
            attention = UnityEngine.Random.Range(10, 111);

            focus = f;
            this.size = size;
            this.template = template;
        }

        public Unit(UnitTemplate template) : this(Util.EnumRandom<Focus>(), template.size, template) { }

        public (FieldInfo, FieldInfo) GetMinMax()
        {
            Type type = typeof(Unit);

            FieldInfo max = (from FieldInfo fi in type.GetFields()
                             where fi.IsDefined(typeof(UnitFieldAttribute))
                             orderby (int)fi.GetValue(this) descending
                             select fi).First();

            FieldInfo min = (from FieldInfo fi in type.GetFields()
                             where fi.IsDefined(typeof(UnitFieldAttribute)) 
                             orderby (int)fi.GetValue(this) ascending
                             select fi).First();

            return (min, max);
        }

        public override string ToString()
        {
            return $"[\nsize: {size},\nfocus: {(int)focus},\nmelee: {meleePower},\nranged: {rangedPower},\nattention: {attention}\n]";
        }
    }
}
