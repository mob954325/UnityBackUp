using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // components
    PlayerInputActions actions;
    Animator animator;
    Rigidbody rigid;

    // player input values
    public Vector3 playerInput;
    public Vector2 mouseInput;

    // player Stats
    public float moveSpeed = 5.0f;
    public float rotSpeed = 0.3f;
    [Range(0f,1f)]
    public float rotationPower = 5.0f;

    // player's Transform objects
    public Transform followTransform;
    public Transform playerModel;

    public float rotationLerp = 1.2f;

    // player movement
    [Header("Movement")]
    Vector3 moveDirection;
    float inputVertical = 0f;
    float inputHorizontal = 0f;

    // player animator
    readonly int inputVertical_String = Animator.StringToHash("Vertical"); // input.z
    readonly int inputHorizontal_String = Animator.StringToHash("Horizontal"); // input.x

    void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        playerModel = transform.GetChild(0);
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // 커서 고정
        //Cursor.visible = false; // 커서 가리기

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
        GetPlayerMoveInput();
        PlayerRotate();
        rotateCamera();

        PlayAnimMove();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector3>();
    }

    void GetPlayerMoveInput()
    {
        // CALL AFTER playerMove()
        inputVertical = playerInput.z;
        inputHorizontal = playerInput.x;
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    void rotateCamera()
    {
        #region Follow Transform Rotataion

        // Rotate Player based on input
        //transform.rotation *= Quaternion.AngleAxis(mouseInput.x * rotationPower, Vector3.up);

        #endregion

        #region Vertical Rotation

        // Rotate the follow target transfrom based on input

        // leftright
        followTransform.transform.rotation *= Quaternion.AngleAxis(mouseInput.x * rotationPower, Vector3.up);

        // updown
        followTransform.transform.rotation *= Quaternion.AngleAxis(mouseInput.y * -rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        // Clamp the up/down rotation
        if(angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;

        #endregion

        // Set the player rotation based on the look transform
        //transform.rotation = Quaternion.Euler(0, followTransform.transform.eulerAngles.y, 0);

        // Reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }

    /// <summary>
    /// player model rotate
    /// </summary>
    void PlayerRotate()
    {
        Vector3 rotDirection = Vector3.zero;
        rotDirection.x = inputHorizontal;
        rotDirection.z = inputVertical;
        rotDirection.Normalize();

        if(rotDirection.magnitude > 0.01f)
        {
            float lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg;

           // float angle = Mathf.LerpAngle(playerModel.rotation.eulerAngles.y, lookAngle, rotSpeed);
            float angle = Mathf.LerpAngle(playerModel.localRotation.eulerAngles.y, lookAngle, rotSpeed);

            //playerModel.rotation = Quaternion.Euler(0, angle, 0); // rotate Player model
            playerModel.localRotation = Quaternion.Euler(0, angle, 0); // rotate Player model
        }
    }

    void playerMove()
    {
        // calculate movement direction
        moveDirection = transform.forward * inputVertical + transform.right * inputHorizontal;

        //rigid.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        rigid.MovePosition(rigid.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void PlayAnimMove()
    {
        // check input value
        animator.SetFloat(inputVertical_String, inputVertical);
        animator.SetFloat(inputHorizontal_String, inputHorizontal);
    }
}
