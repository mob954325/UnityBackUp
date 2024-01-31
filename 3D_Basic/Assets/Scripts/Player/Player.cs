using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    PlayerInput playerAction; // WASD , Space , F(Use)
    Rigidbody rigid;
    Animator animator;

    // input
    public Vector3 inputVec = Vector3.zero;

    /// <summary>
    /// Move Direction (1 : forward, -1 : backward, 0 : stop)
    /// </summary>
    float moveDirection = 0.0f;

    /// <summary>
    /// RotateDirection (1 : right , -1 : left, 0 : stop)
    /// </summary>
    float rotateDirection = 0.0f;


    // Player Stats
    /// <summary>
    /// Player Move Speed
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// Player Rotate Speed
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// Player JumpPower
    /// </summary>
    public float jumpPower = 6.0f;

    /// <summary>
    /// Check Player is jumping
    /// </summary>
    bool isJumping = false;

    /// <summary>
    /// Jump CoolDown
    /// </summary>
    public float jumpCooltime = 5.0f;

    /// <summary>
    ///  ���� �ִ� ��Ÿ��
    /// </summary>
    float jumpCoolRemain = -1.0f;

    /// <summary>
    /// ������ �������� Ȯ���ϴ� ������Ƽ(�������� �ƴϰ� ��Ÿ���� �� ������.)
    /// </summary>
    bool IsJumpAvailable =>  !isJumping && (jumpCoolRemain < 0.0f);

    /// <summary>
    /// Animator Hash Values
    /// </summary>
    readonly int isMoveHash = Animator.StringToHash("isMove");

    void Awake()
    {
        //playerAction = new PlayerInput();
        playerAction = new(); // ������ Ÿ���� �ָ��ϸ� �ȵ�
        rigid = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        playerAction.Player.Enable();
        playerAction.Player.Move.performed += OnMoveInput;
        playerAction.Player.Move.canceled += OnMoveInput;
        playerAction.Player.Jump.performed += OnJumpInput;
        playerAction.Player.Use.performed += OnUseInput;
    }

    void OnDisable()
    {
        playerAction.Player.Use.performed -= OnUseInput;
        playerAction.Player.Jump.performed -= OnJumpInput;
        playerAction.Player.Move.canceled -= OnMoveInput;
        playerAction.Player.Move.performed -= OnMoveInput;
        playerAction.Player.Disable();
    }

    private void OnMoveInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJumpInput(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        Jump();
    }

    private void OnUseInput(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        jumpCoolRemain -= Time.deltaTime;

        if(jumpCoolRemain < 0)
        {
            //Debug.Log("Jump Ready");
        }
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
       // JumpTimer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    /// <summary>
    /// �̵� ó�� �Լ�
    /// </summary>
    /// <param name="input">Input Direction</param>
    /// <param name="isMove">������ : ture , �ȿ����� : false</param>
    void SetInput(Vector2 input, bool isMove)
    {
        rotateDirection = input.x;
        moveDirection = input.y;

        animator.SetBool(isMoveHash, isMove);
    }

    /// <summary>
    /// ���� �̵� ó�� �Լ�
    /// </summary>
    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDirection * transform.forward);
    }

    void Rotate()
    {
        // fixedUpdate���� �߰��� ȸ���� ȸ��

        // ȸ���� ǥ���ϴ� Ŭ���� : Quaternion
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection,
                                                 transform.up);

        // ���� ȸ������ rotate��ŭ �߰��� ȸ��
        rigid.MoveRotation(rigid.rotation * rotate);

    }

    /// <summary>
    /// ���� ������ ó���ϴ� �Լ�
    /// </summary>
    void Jump()
    {
        if (IsJumpAvailable) // ������ ������ ���� ����
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse); // �������� jumpPower��ŭ ���� ���ϱ�
            jumpCoolRemain = jumpCooltime; // ���� ��Ÿ�� �ʱ�ȭ
            isJumping = true; // ���� �ߴٰ� ǥ��
        } 
    }
}