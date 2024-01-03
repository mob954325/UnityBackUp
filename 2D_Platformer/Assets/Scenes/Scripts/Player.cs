using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerInput action;

    public float speed;
    public float jumpPower;

    Vector2 inputVec = Vector2.zero;

    Rigidbody2D rigid;
    Collision2D coll;

    void Awake()
    {
        action = new PlayerInput();

        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        action.Player.Enable();
        action.Player.Move.performed += OnMove;
        action.Player.Move.canceled += OnMove;
        action.Player.Move.performed += OnJump;

    }

    void OnDisable()
    {
        action.Player.Move.performed -= OnJump;
        action.Player.Move.canceled -= OnMove;
        action.Player.Move.performed -= OnMove;
        action.Player.Disable();
    }



    void Update()
    {
    }

    void FixedUpdate()
    {

        transform.Translate(inputVec * Time.fixedDeltaTime * speed);
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnJump : Key Down");
        }
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 vec = context.ReadValue<Vector2>();

        inputVec = vec;
    }





}
