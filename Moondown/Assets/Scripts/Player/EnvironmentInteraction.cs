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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moondown.Environment;
using Moondown.Player.Movement;
using Moondown.UI;
using Moondown.Utility;

namespace Moondown.Player
{
    public class EnvironmentInteraction : MonoBehaviour
    {
        public static InteractionResult Result
        {
            get
            {
                return Player.Instance.GetComponent<EnvironmentInteraction>().GetResult();
            }
        }

        private InteractionResult GetResult()
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();

            RaycastHit2D[] hits = Physics2D.BoxCastAll(
                transform.position,
                (Vector2)collider.bounds.size + new Vector2(0.1f, 0.1f),
                0.0f,
                Vector2.zero
            );

            InteractionResult result = new InteractionResult();

            foreach (RaycastHit2D hit in hits)
            {
                GameObject go = hit.transform.gameObject;

                if (go.Has<EnvironmentBehaviour>())
                {
                    EnvironmentBehaviour eb = go.GetComponent<EnvironmentBehaviour>();

                    result += new InteractionResult
                    {
                        health = eb.healthModifier,
                        charge = eb.chargeModifier,
                        hasBeenHit = eb.reset,
                        climbable = go.CompareTag("climbable")
                    };
                }
                else
                {
                    if (go.CompareTag("climbable"))
                    {
                        
                        result += new InteractionResult
                        {
                            health = 0,
                            charge = 0,
                            hasBeenHit = false,
                            climbable = true
                        };
                    }
                }
            }

            return result;
        }


    }
}