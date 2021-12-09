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

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _playerSpeed;
    private MainControls _controls;
    private Rigidbody2D _rigidBody;

    private bool isMovementPressed = false;
    private float movementAxis;

    private bool grounded = true;

    private void Awake()
    {
        _controls = new MainControls();

        _controls.Player.AttackMeele.performed += ctx => AttackMeele();
        _controls.Player.Jump.performed += ctx => Jump();
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
    private void OnDisable() => _controls.Enable();

    void AttackMeele()
    {
        Debug.Log("attacked");
    }

    void Jump()
    {
        if (grounded)
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y + 12f);
    }

    void Move(float direction)
    {
        _rigidBody.velocity = new Vector2(_playerSpeed * direction, _rigidBody.velocity.y);
    }

    void MoveCancelled()
    {
        if (grounded)
            _rigidBody.velocity = new Vector2(0f, _rigidBody.velocity.y);
    }

    bool IsGrounded()
    {
        // boxcast
        return true;
    }
}
