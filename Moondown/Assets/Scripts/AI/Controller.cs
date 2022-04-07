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

using Moondown.AI.Enemy;
using Moondown.AI.Enemy.Modules.Sensor;
using System.Collections.Generic;

namespace Moondown.AI
{
    using Moondown.Player;
    using UnityEngine;

    public class Controller 
    {
        private ControllerGroup group;

        public string Name => group.name;
        public Unit[] Units => group.units.ToArray();
        public bool Searching { get; private set; }

        private bool hasLineOfSight = false;

        private Formation leftFormation, rightFormation;

        public Controller(ControllerGroup group)
        {
            this.group = group;

            SetTarget(null, group.units.ToArray());
            GameManager.Instance.Controllers.Add(this);

            GameManager.Tick += Tick;
        }

        private void Tick()
        {
            CheckSearch();
            EvaluateTargets();
            SetCommands();
            CreateFormations();
        }

        private void CheckSearch()
        {
            hasLineOfSight = false;

            foreach (Unit unit in Units)
            {
                if (hasLineOfSight)
                    break;

                hasLineOfSight = unit.Search().found;
            }

            Searching = !hasLineOfSight;

            if (!hasLineOfSight)
                SetStates(new UnitState.Searching(null), Units);

        }

        private void EvaluateTargets()
        {
            // TODO: add code
        }

        private void SetCommands()
        {
            if (hasLineOfSight)
            {
                SetStates(new UnitState.Engaged(null), Units);
            }
        }

        private void CreateFormations()
        {
            // TODO: Decide which direction to create the formation

            float range = 3f;

            List<Unit> formationUnits = group.units;

            Formation formation = new Formation(range, Player.Instance.transform.position, formationUnits);

            leftFormation = formation;
        }

        public void SetStates(UnitState state, params Unit[] units)
        {
            foreach (Unit unit in units)
            {
                if (unit.State.GetType().Name == state.GetType().Name)
                    continue;

                unit.SetState(state.SetUnit(unit));
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