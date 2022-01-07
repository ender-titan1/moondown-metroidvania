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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moondown.Utility;
using System;
using System.Linq;

namespace Moondown.Player.Movement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance { get; private set; }

        private const float MAX_ANGLE = 45f;

        private MainControls controls;
        private Rigidbody2D rigidBody;
        public Facing facing;

        #region Jumping 
        [Header("Jump & Wall Jump")]

        [SerializeField]
        private float jumpVelocity;

        [SerializeField]
        private float wallJumpVelocity;

        private bool grounded;
        #endregion

        #region Movement
        [Header("Movement")]

        [SerializeField]
        private LayerMask mask;
        
        [SerializeField]
        private float playerSpeed;

        private bool isMovementPressed;
        private int movementAxis;

        private PhysicsMaterial2D groundMaterial;
        #endregion

        #region Dashing

        private bool canDash = true;
        private bool isDashing;

        private float playerYPos;

        [Header("Dashing")]
        [SerializeField]
        private float dashVelocity;
        [SerializeField]
        private float dashDuration;
        [SerializeField]
        private float dashCooldown;
        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            controls = new MainControls();
            rigidBody = gameObject.GetComponent<Rigidbody2D>();

            controls.Player.Jump.performed += _ => Jump();

            controls.Player.DashRight.performed += _ => Dash(1);
            controls.Player.DashLeft.performed += _ =>  Dash(-1);
            controls.Player.DashForController.performed += _ => GamepadDash();

            controls.Player.Movement.performed += ctx => 
            { 
                isMovementPressed = true;
                movementAxis = ctx.ReadValue<float>().ToAxis(movementAxis == 0 ? 1 : movementAxis); 
            };

            controls.Player.Movement.canceled += _ => 
            { 
                isMovementPressed = false; 
                MoveCancelled(); 
            };

        }

        private void FixedUpdate()
        {
            // jumping
            grounded = IsGrounded();

            // moving
            if (isMovementPressed)
                Move(movementAxis);

            // dashing
            if (isDashing)
            {
                if (gameObject.transform.position.y != playerYPos)
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, playerYPos);
            }

            if (grounded)
                canDash = true;
        }

        private void OnEnable() => controls.Enable();
        private void OnDisable() => controls.Disable();

        #region movement

        void Jump()
        {
            if (grounded)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + jumpVelocity);
                return;
            }

            bool wallJump = CanWallJump(facing);
            if (wallJump)
                WallJump(facing);
        }

        void Move(float direction)
        {
            facing = (Facing)direction;

            rigidBody.velocity = new Vector2(
                playerSpeed * direction - (grounded ? (groundMaterial.friction * direction) : 0f),
                rigidBody.velocity.y
            );
        }

        void MoveCancelled()
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }

        void WallJump(Facing direction)
        {
            rigidBody.velocity = new Vector2(0, wallJumpVelocity);
        }

        #region Dash

        void Dash(float direction)
        {
            if (!canDash)
                return;

            canDash = false;
            playerYPos = gameObject.transform.position.y;
            isDashing = true;

            rigidBody.AddForce(new Vector2(dashVelocity * (int)facing * 600, 0f));
            controls.Disable();
            facing = (Facing)direction;

            Invoke(nameof(CancelDash), dashDuration);
            Invoke(nameof(RefreshDash), dashCooldown);
        }

        void CancelDash()
        {
            rigidBody.velocity = Vector2.zero;
            controls.Enable();

            isDashing = false;
        }

        void RefreshDash()
        {
            if (grounded)
                canDash = true;
        }

        void GamepadDash()
        {
            if (facing == Facing.LEFT)
                Dash(-1);
            else
                Dash(1);
        }

        #endregion

        #endregion

        bool IsGrounded()
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            RaycastHit2D[] hits = Physics2D.BoxCastAll(pos - new Vector2(0, 0.05f), collider.size, 0, Vector2.down, collider.size.y, mask);

            foreach (RaycastHit2D item in hits)
            {
                if (item.transform.CompareTag("Player"))
                    continue;

                float fixedangle = Mathf.Abs(Mathf.Atan2(item.normal.x, item.normal.y) * (180 / Mathf.PI));

                if (fixedangle < MAX_ANGLE)
                {
                    groundMaterial = item.collider.sharedMaterial;
                    return true;
                }
            }

            return false;
        }

        bool CanWallJump(Facing direction)
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            
            RaycastHit2D[] hits = Physics2D.BoxCastAll(
                pos - new Vector2(0.2f * (int)direction, 0),
                collider.size,
                0,
                direction == Facing.LEFT ? Vector2.left : Vector2.right,
                collider.size.y,
                mask
            );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                    continue;

                if (hit.collider.CompareTag("can wall jump"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}