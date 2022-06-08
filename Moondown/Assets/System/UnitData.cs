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

namespace Moondown.Sys
{
    public struct UnitData
    {
        public enum Behaviour
        {
            Regular,
            Broken,
            Agitated,
            Cautious,
            Ancient,
            Oblivious
        }

        public enum Focus
        {
            RangeFocused,
            MeleeFocused,
            SupportFocused,
            Neutral
        }

        public enum Aesthetics
        {
            Clean,
            Dusty,
            Sandy,
            Dirty,
            Worn,
            Clan,
            Frozen
        }
        
        public enum Physical
        {
            Regular,
            Armoured,
            Exposed,
            Weakened,
            Clan
        }

        public Behaviour behaviour;
        public Focus focus;
        public Aesthetics aesthetics;
        public Physical physical;

        public UnitData(Behaviour b, Focus f, Aesthetics a, Physical p)
        {
            behaviour = b;
            focus = f;
            aesthetics = a;
            physical = p;
        }
    }    
}