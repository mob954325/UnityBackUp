using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // components
    PlayerInputActions actions;
    Rigidbody rigid;

    // player input values
    public Vector3 playerInput;
    public Vector2 mouseInput;

    // player Stats
    public float speed = 5.0f;
    public Transform orientation;

    // player movement
    [Header("Movement")]
    Vector3 moveDirection;

    void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 커서 고정
        Cursor.visible = false; // 커서 가리기

        rigid.freezeRotation = true;
    }

    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.Jump.performed += OnJumpInput;
        actions.Player.Look.performed += OnLookInput;
        actions.Player.Look.canceled += OnLookInput;
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    void OnDisable()
    {
        actions.Player.Look.canceled -= OnLookInput;
        actions.Player.Look.performed -= OnLookInput;
        actions.Player.Jump.performed -= OnJumpInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Disable();
    }

    void FixedUpdate()
    {
        playerMove();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector3>();
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    void cameraControl()
    {

    }

    void playerMove()
    {
        // calculate movement direction
        moveDirection = transform.forward * playerInput.z + transform.right * playerInput.x;

        rigid.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }
}
