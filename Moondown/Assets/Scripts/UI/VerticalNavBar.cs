/*
    A script to control basic player movement
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
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.UI
{
    public class VerticalNavBar : MonoBehaviour
    {
        private GameObject[] selection;
        private GameObject selected;
        private bool _enabled;
        private int index;

        private MainControls controls;

        private int axis;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;

                if (value)
                {
                    controls.Enable();
                    InvokeRepeating(nameof(RepeatSelect), 0, 0.3f);
                }
                else
                {
                    foreach (GameObject @object in selection)
                    {
                        @object.GetComponent<NavBarElement>().Select(false);
                    }

                    controls.Disable();

                    CancelInvoke(nameof(RepeatSelect));
                }
            }
        }

        private void Awake()
        {
            List<GameObject> objects = new List<GameObject>();

            foreach (Transform transform in gameObject.transform)
                objects.Add(transform.gameObject);

            selection = objects.ToArray();

            controls = new MainControls();

            controls.UI.Up.performed += _ => Select(false);
            controls.UI.Down.performed += _ => Select(true);

            controls.UI.Down.performed += _ => axis--;
            controls.UI.Down.canceled += _ =>  axis++;

            controls.UI.Up.performed += _ => axis++;
            controls.UI.Up.canceled += _ =>  axis--;

            controls.UI.Select.performed += _ =>
            {
                selected.GetComponent<NavBarElement>().Activate();
            };
        }

        void RepeatSelect()
        {
            Debug.Log(axis);

            if (axis == 0)
                return;

            Select(axis > 0);
        }
        
        void Select(bool direction)
        { 
            if (selected != null && selected.Has<NavBarElement>())
                selected.GetComponent<NavBarElement>().Select(false);

            if (direction)
            {
                index++;

                if (index > selection.Length - 1)
                    index = 0;
            }
            else
            {
                index--;

                if (index < 0)
                    index = selection.Length - 1;
            }

            selected = selection[index];

            if (selected.Has<NavBarElement>())
                selected.GetComponent<NavBarElement>().Select(true);

        }

    }
}