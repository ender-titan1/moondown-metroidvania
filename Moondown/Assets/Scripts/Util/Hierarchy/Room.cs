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

using Moondown.AI;

namespace Moondown.Utility.Hierarchy
{
    public class Room : IControllerZone
    {
        public IControllerZone Parent { get; set; }
        public IControllerZone[] Children { get; set; }

        private string name;

        public Room(IControllerZone parent, string name)
        {
            Parent = parent;
            this.name = name;
        }

        public string GetName()
        {
            if (Parent == null)
                return name;

            return $"{Parent.GetName()}:{name}"; 
        }

        public override string ToString()
        {
            return $"{{name: {GetName()}}}";
        }
    }
}