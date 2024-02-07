using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IAlive
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
    ///  남아 있는 쿨타임
    /// </summary>
    float jumpCoolRemain = -1.0f;

    /// <summary>
    /// 플레이어의 생존 여부
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 점프가 가능한지 확인하는 프로퍼티(점프중이 아니고 쿨타임이 다 지났다.)
    /// </summary>
    bool IsJumpAvailable =>  !isJumping && (jumpCoolRemain < 0.0f);

    /// <summary>
    /// Animator Hash Values
    /// </summary>
    readonly int isMoveHash = Animator.StringToHash("isMove");
    readonly int UseHash = Animator.StringToHash("Use");
    readonly int DieHash = Animator.StringToHash("Die");

    /// <summary>
    /// 플레이어의 사망을 알리는 델리게이트
    /// </summary>
    public Action onDie;

    void Awake()
    {
        //playerAction = new PlayerInput();
        playerAction = new(); // 데이터 타입이 애매하면 안됨
        rigid = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>();
        checker.onItemUse += (IInteracable) => IInteracable.Use();
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
        animator.SetTrigger(UseHash);
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
    /// 이동 처리 함수
    /// </summary>
    /// <param name="input">Input Direction</param>
    /// <param name="isMove">움직임 : ture , 안움직임 : false</param>
    void SetInput(Vector2 input, bool isMove)
    {
        rotateDirection = input.x;
        moveDirection = input.y;

        animator.SetBool(isMoveHash, isMove);
    }

    /// <summary>
    /// 실제 이동 처리 함수
    /// </summary>
    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * moveDirection * transform.forward);
    }

    void Rotate()
    {
        // fixedUpdate에서 추가로 회전할 회전

        // 회전을 표현하는 클래스 : Quaternion
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection,
                                                 transform.up);

        // 현재 회전에서 rotate만큼 추가로 회전
        rigid.MoveRotation(rigid.rotation * rotate);

    }

    /// <summary>
    /// 실제 점프를 처리하는 함수
    /// </summary>
    void Jump()
    {
        if (IsJumpAvailable) // 점프가 가능할 때만 점프
        {
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse); // 위쪽으로 jumpPower만큼 힘을 더하기
            jumpCoolRemain = jumpCooltime; // 점프 쿨타임 초기화
            isJumping = true; // 점프 했다고 표시
        } 
    }

    /// <summary>
    /// 사망처리용 함수
    /// </summary>
    public void Die()
    {
        if(isAlive)
        {
            Debug.Log("Player 죽었음");

            animator.SetTrigger(DieHash);
            playerAction.Disable();


            Transform head = transform.GetChild(0); // Head Object
            rigid.constraints = RigidbodyConstraints.None; // 물리 잠금을 전부 해제하기
            rigid.AddForceAtPosition(-transform.forward, head.position, ForceMode.Impulse);
            rigid.AddTorque(transform.up * 1.5f, ForceMode.Impulse);

            onDie?.Invoke();

            // 죽는 애니메이션이 나온다.
            // 더 이상 조종이 안되어야한다.
            // 대굴대굴 구른다. (뒤로 넘어가면서 y축으로 스핀을 먹는다.)
            // 죽었다고 신호보내기 => 델리게이트
            isAlive = false;
        }
    }
}