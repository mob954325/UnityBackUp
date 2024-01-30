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
    public GameObject target;

    // player Stats
    public float speed = 5.0f;
    public float rotSpeed = 0.05f;

    void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.Jump.performed += OnJumpInput;      
    }
    void OnDisable()
    {
        actions.Player.Jump.performed -= OnJumpInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Disable();
    }

    void FixedUpdate()
    {
        moveControl();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    void moveControl()
    {
        // 움직이는 방향
        Vector3 moveDir = new Vector3(playerInput.x, 0, playerInput.y);
        
        // 회전 방향
        Vector3 rotDir = Vector3.zero;
        Vector3 effectiveDirection = Vector3.zero;
        rotDir.x = moveDir.x;
        rotDir.z = moveDir.z;
        rotDir.Normalize();

        if (rotDir.magnitude > 0.01f)
        {
            float lookAngle = Mathf.Atan2(rotDir.x, rotDir.z) * Mathf.Rad2Deg; // 각도 변환
            float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, lookAngle, rotSpeed); // 현재각도, 플레이어가 입력한 각도
            transform.rotation = Quaternion.Euler(0, angle, 0); // rotate 설정
        }

        //effectiveDirection = Vector3.Lerp(effectiveDirection, rotDir, 0.02f);
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveDir * speed);
    }

}
