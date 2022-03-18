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

using UnityEngine;

namespace Moondown.AI.Event
{
    public class MoondownEvent
    {
        private Unit initiator;
        private Controller controller;
        private ControllerGroup group;

        private MoondownEvent(Unit initiator, Controller controller, ControllerGroup group)
        {
            this.initiator = initiator;
            this.controller = controller;
            this.group = group;

        }

        private EventResolution Resolve()
        {
            if (group.units.Count == 0)
                return EventResolution.Failed;
            else
                return EventResolution.PlayerEliminated;
        }

        public static MoondownEvent Of(Unit unit)
        {
            unit.Controller ??= new Controller();
           
            MoondownEvent @event = new MoondownEvent(unit, unit.Controller, unit.Group);
            GameManager.Instance.Events.Add(@event);

            return @event;

        }
    }
}