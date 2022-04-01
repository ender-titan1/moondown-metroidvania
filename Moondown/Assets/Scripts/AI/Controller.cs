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

using System.Threading.Tasks;
using System.Timers;
using Moondown.AI.Enemy.Modules.Sensor;
using System.Collections.Generic;

namespace Moondown.AI
{
    using Moondown.AI.Enemy;
    using Moondown.Player;

    public class Controller 
    {
        private ControllerGroup group;

        public string Name => group.name;
        public Unit[] Units => group.units.ToArray();

        public List<IEngagable> potentialTargets = new List<IEngagable>();

        public bool Searching { get; private set; }

        public Controller(ControllerGroup group)
        {
            this.group = group;

            potentialTargets.Add(Player.Instance);

            SetTarget(null, group.units.ToArray());
            GameManager.Instance.Controllers.Add(this);

            GameManager.Tick += Tick;
        }

        private void Tick()
        {
            bool hasLineOfSight = false;

            foreach (Unit unit in Units)
            {
                if (hasLineOfSight)
                    break;

                hasLineOfSight = unit.GetComponentInChildren<VisualSensor>().Search().found;
            }

            Searching = !hasLineOfSight;

            if (!hasLineOfSight)
            {
                InitiateSearch(Units);
            }
        }

        public void InitiateSearch(params Unit[] units)
        {
            foreach (Unit unit in units)
            { 
                unit.SetState<UnitState.Searching>();
            }
        }

        public void SetTarget(IEngagable target, params Unit[] units)
        {
            target ??= Player.Instance;

            foreach (Unit unit in units)
                unit.SetTarget(target);

        }
    }
}