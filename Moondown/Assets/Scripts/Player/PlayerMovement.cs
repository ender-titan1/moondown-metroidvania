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

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private const float MAX_ANGLE = 45f;

    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private float _jumpVelocity;

    private MainControls _controls;
    private Rigidbody2D _rigidBody;

    private bool isMovementPressed;
    private float movementAxis;

    private bool grounded;
    private PhysicsMaterial2D groundMaterial;

    private void Awake()
    {
        _controls = new MainControls();

        _controls.Player.AttackMeele.performed += _ => AttackMeele();
        _controls.Player.Jump.performed += _ => Jump();
        _controls.Player.Movement.performed += ctx => { isMovementPressed = true; movementAxis = ctx.ReadValue<float>(); };
        _controls.Player.Movement.canceled += _ => { isMovementPressed = false; MoveCancelled(); };

        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        grounded = IsGrounded();
        
        if (isMovementPressed)
            Move(movementAxis);
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    void AttackMeele()
    {
        Debug.Log("attacked");
    }

    #region movement

    void Jump()
    {
        if (grounded)
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y + _jumpVelocity);
    }

    void Move(float direction)
    {
       _rigidBody.velocity = new Vector2(
           _playerSpeed * direction - (grounded ? (groundMaterial.friction * direction) : 0f),
           _rigidBody.velocity.y
       );
    }

    void MoveCancelled()
    {
        _rigidBody.velocity = new Vector2(0f, _rigidBody.velocity.y);
    }

    #endregion

    bool IsGrounded()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(pos - new Vector2(0, 1f), collider.size, 0, Vector2.down, collider.size.y);

        foreach (RaycastHit2D item in hits)
        {
            if (item.transform.CompareTag("Player")) 
                continue;

            float angle = Mathf.Atan2(item.normal.x, item.normal.y) * (180 / Mathf.PI);
            float fixedangle = Mathf.Abs(angle);

            if (fixedangle < MAX_ANGLE)
            {
                groundMaterial = item.collider.sharedMaterial;
                return true;
            }
        }

        return false;
    }
}
