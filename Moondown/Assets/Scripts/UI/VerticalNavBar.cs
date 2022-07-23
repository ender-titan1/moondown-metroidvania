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
using System.Collections;
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
        private bool repeat;

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
                }
                else
                {
                    HideAll();

                    controls.Disable();
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

            controls.UI.Select.performed += _ => selected.GetComponent<NavBarElement>().Activate();

        }

        private void Start()
        {
            StartCoroutine(nameof(Repeat));
        }

        // This code needs to be improved
        IEnumerator Repeat()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.2f);

                if (Enabled && axis != 0)
                {
                    if (repeat && Enabled && axis != 0)
                    {
                        Select(axis < 0);
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(1.0f);

                        if (Enabled && axis != 0)
                        {
                            Select(axis < 0);
                            repeat = true;
                        }
                    }
                }
                else
                {
                    repeat = false;
                }
            }
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

        public void HideAll()
        {
            foreach (GameObject @object in selection)
            {
                @object.GetComponent<NavBarElement>().Select(false);
            }
        }
    }
}