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
using UnityEngine;
using Moondown.Utility;
using Moondown.UI;
using System;

namespace Moondown.Player.Movement
{
    // TODO: Jump Buffer
    // TODO: Coyote Time
    // TODO: Encapsulate fields?
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public enum Mode
        {
            Normal,
            Climbing
        }

        public static PlayerMovement Instance { get; private set; }

        private const float MAX_ANGLE = 45f;
        private const int MAX_JUMPS = 1;
        private readonly Vector2 RC_OFFSET = new Vector2(0, 0.0005f);

        public MainControls controls;
        private Rigidbody2D rigidBody;
        public Mode mode;
        public Facing facing = Facing.Right;
        private float gravity = 2.5f;

        private int climbingAxis;

        [SerializeField] private float climbingSpeed;

        #region Jumping 
        [Header("Jump & Wall Jump")]

        [SerializeField]
        private float jumpVelocity;

        [SerializeField]
        private float wallJumpVelocity;

        private bool canJump;

        [SerializeField]
        private int jumps;

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

        public bool IsGrounded => canJump;

        private void OnEnable() => controls.Enable();
        private void OnDisable() => controls.Disable();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            controls = new MainControls();
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
            SetupEvents();

            Config config = Config.Load();

            if (config != null)
            {
                if (config.playerDashVelocity != 0)
                    dashVelocity = config.playerDashVelocity;
                
                if (config.playerPhysics.gravity != 0)
                {
                    rigidBody.gravityScale = config.playerPhysics.gravity;
                    gravity = config.playerPhysics.gravity;
                }

                if (config.playerJumpVelocity != 0)
                    jumpVelocity = config.playerJumpVelocity;
                
                if (config.playerWallJumpVelocity != 0)
                    wallJumpVelocity = config.playerWallJumpVelocity;

                if (config.playerPhysics.mass != 0)
                    rigidBody.mass = config.playerPhysics.mass;
                
                if (config.playerPhysics.linearDrag != 0)
                    rigidBody.drag = config.playerPhysics.linearDrag;

                if (config.playerPhysics.angularDrag != 0)
                    rigidBody.angularDrag = config.playerPhysics.angularDrag;

            }
        }

        private void SetupEvents()
        {
            // Jumping & Climbing controls
            controls.Player.Jump.performed += _ =>
            {
                Jump();
            };

            controls.Player.Jump.canceled += _ =>
            {
                if (mode == Mode.Climbing)
                    ClimbVertical(0); // Stop climbing
            };

            // Moving controls
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

            // Dashing controls
            controls.Player.Dash.performed += _ => Dash(movementAxis == 0 ? 1 : movementAxis);

            // Climbing controls
            controls.Player.ClimbVertical.performed += ctx =>
            {
                climbingAxis = ctx.ReadValue<float>().ToAxis(climbingAxis == 0 ? 1 : climbingAxis);
                Climb();
            };

            controls.Player.ClimbVertical.canceled += _ =>
            {
                climbingAxis = 0;
                Climb();
            };

            controls.Player.ClimbDown.performed += _ =>
            {
                ClimbVertical(-1);
            };

            controls.Player.ClimbDown.canceled += _ =>
            {
                ClimbVertical(0);
            };

            // Interaction controls
            controls.Player.Interact.performed += _ =>
            {
                Interact();
            };
        }

        private void FixedUpdate()
        {
            // jumping
            canJump = CheckGround();

            if (canJump)
                jumps = MAX_JUMPS;

            // moving
            if (isMovementPressed && !UIManager.Instance.IsInInterface)
                Move(movementAxis);

            // dashing
            if (isDashing)
            {
                if (gameObject.transform.position.y != playerYPos)
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x, playerYPos);
            }

            if (canJump)
                canDash = true;
        }

        private void Update()
        {
            InteractionResult res = EnvironmentInteraction.Result;

            if (!res.climbable && mode == Mode.Climbing)
                mode = Mode.Normal;

            rigidBody.gravityScale = mode.HasGravity() * gravity;
        }

        #region Movement

        void Jump()
        {
            if (mode == Mode.Climbing)
            {
                ClimbVertical(1);
                return;
            }

            bool wallJump = CanWallJump(facing);
            if (wallJump && !UIManager.Instance.IsInInterface)
            {
                WallJump(facing);
                return;
            }

            if (jumps > 0 && !UIManager.Instance.IsInInterface)
            {
                if (!canJump)
                    rigidBody.velocity = Vector2.zero;

                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y + jumpVelocity);
                jumps--;
                return;
            }

        }

        void Move(float direction)
        {
            facing = (Facing)direction;

            if (mode == Mode.Normal)
            {
                rigidBody.velocity = new Vector2(
                    playerSpeed * direction - (canJump && groundMaterial != null ? (groundMaterial.friction * direction) : 0f),
                    rigidBody.velocity.y
                );
            }
            else if (mode == Mode.Climbing)
            {
                rigidBody.velocity = new Vector2(
                    climbingSpeed * direction,
                    rigidBody.velocity.y
                );
            }
        }

        void MoveCancelled()
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
        }

        void WallJump(Facing direction)
        {
            rigidBody.velocity = new Vector2(0, wallJumpVelocity);
        }

        public void Climb()
        {
            if (mode == Mode.Climbing)
                ClimbVertical(climbingAxis);
        }

        public void ClimbVertical(int direction)
        {
            rigidBody.velocity = new Vector2(
                rigidBody.velocity.x,
                climbingSpeed * direction
            );
        }

        #region Dash

        void Dash(float direction)
        {
            if (!canDash || UIManager.Instance.IsInInterface || mode == Mode.Climbing)
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

        public void CancelDash()
        {
            rigidBody.velocity = Vector2.zero;
            controls.Enable();

            isDashing = false;
        }

        public void RefreshDash()
        {
            if (canJump)
                canDash = true;
        }

        #endregion
        #endregion

        void Interact()
        {
            if (EnvironmentInteraction.Result.climbable)
            {
                switch (mode)
                {
                    case Mode.Normal:
                        rigidBody.velocity = Vector2.zero;
                        mode = Mode.Climbing;
                        break;
                    case Mode.Climbing:
                        mode = Mode.Normal;
                        break;
                    default:
                        break;
                }
            }
        }

        bool CheckGround()
        {
            BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

            Vector2 size = collider.size;
            RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position - RC_OFFSET, size, 0, Vector2.down, collider.size.y, mask);

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
                pos - new Vector2(0.001f * (int)direction, 0),
                collider.size,
                0,
                direction == Facing.Left ? Vector2.left : Vector2.right,
                0.1f,
                mask
            );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                    continue;

                if (hit.collider.CompareTag("can wall jump"))
                    return true;
            }

            return false;
        }
    }
}