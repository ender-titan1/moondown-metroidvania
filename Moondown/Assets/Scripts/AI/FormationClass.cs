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

namespace Moondown.AI.Enemy
{
    public enum FormationClass
    {
        Protected = -10,        // Protected by the formation.
        Support = 0,            // Stays at the very back and only intervienes when neccessary.
        Backline = 5,           // Heavier, ranged units that stay at the back.
        Frontline = 10,         // Most units. Stays at the front of the battle, as close to the player as possible.
        FrontlineShield = 15,   // Shield / cover units that serve as shields that protect the FrontLine class units.
        PostFrontLine = 25      // Units that go furter away from the formation.
    }
}