using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    /// 현재 이동 속도
    /// </summary>
    float currentMoveSpeed = 5.0f;

    /// <summary>
    /// Player Rotate Speed
    /// </summary>
    public float rotateSpeed = 180.0f;

    /// <summary>
    /// Player JumpPower
    /// </summary>
    public float jumpPower = 6.0f;

    /// <summary>
    /// Check Player is in air
    /// </summary>
    bool InAir
    {
        get => GroundCount < 1;
    }

    /// <summary>
    /// 접촉하고 있는 "Ground" 태그 오브젝트의 개수확인 및 설정용 프로퍼티
    /// </summary>
    int GroundCount
    {
        get => groundCount;
        set
        {
            if (groundCount < 0)
            {
                groundCount = 0;
            }

            groundCount = value;

            if(groundCount < 0)
            {
                groundCount = 0;
            }
        }
    }

    /// <summary>
    /// 접촉하고 있는 "Ground" 태그 오브젝트의 개수
    /// </summary>
    int groundCount = 0;

    /// <summary>
    /// Jump CoolDown
    /// </summary>
    public float jumpCooltime = 5.0f;

    /// <summary>
    ///  남아 있는 쿨타임
    /// </summary>
    float jumpCoolRemain = -1.0f;

    private float JumpCoolRemain
    {
        get => jumpCoolRemain;
        set
        {
            jumpCoolRemain = value;
            onJumpCollTimeChange?.Invoke(JumpCoolRemain / jumpCooltime);
        }
    }

    public Action<float> onJumpCollTimeChange;

    /// <summary>
    /// 플레이어의 생존 여부
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// 점프가 가능한지 확인하는 프로퍼티(점프중이 아니고 쿨타임이 다 지났다.)
    /// </summary>
    bool IsJumpAvailable => !InAir && (JumpCoolRemain < 0.0f) && isAlive;

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

    /// <summary>
    /// 시작했을 때의 플레이어 수명
    /// </summary>
    public float startLifeTime = 10.0f;

    /// <summary>
    /// 현재 플레이어 수명
    /// </summary>
    float lifeTime = 0.0f;

    /// <summary>
    /// 플레이어의 수명을 확인하고 설정하기 위한 프로퍼티
    /// </summary>
    float LifeTime
    {
        get => lifeTime;
        set
        {
            lifeTime = value;
            if (lifeTime <= 0.0f)
            {
                lifeTime = 0.0f; // 수명이 다 되었으면 0.0으로 숫자 정리 및 사망처리
                Die();
            }
            onLifeTimeChange?.Invoke(lifeTime / startLifeTime); // 현재 수명 비율을 알림
        }
    }

    bool isClear = false;

    /// <summary>
    /// 수명이 변경될 때 실행될 델리게이트
    /// </summary>
    public Action<float> onLifeTimeChange;

    void Awake()
    {
        //playerAction = new PlayerInput();
        playerAction = new(); // 데이터 타입이 애매하면 안됨
        rigid = GetComponent<Rigidbody>(); 
        animator = GetComponent<Animator>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>();
        checker.onItemUse += (IInteracable) => IInteracable.Use();
        GameManager.Instance.onGameClear += GameClear;
    }

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        LifeTime = startLifeTime;

        GameManager.Instance.onGameClear += playerAction.Player.Disable;

        // 가상 스틱 연결하기
        VirtualStick stick = GameManager.Instance.Stick;
        if(stick != null)
        {
            stick.OnMoveInput += (inputDelta) => SetInput(inputDelta, inputDelta.sqrMagnitude > 0.0025f);// 이동 입력전달
            onDie += stick.Stop; // 플레이어 죽으면 정지
        }

        VirtualButton jump = GameManager.Instance.JumpButton;
        if(jump != null)
        {
            jump.onClick += Jump; // 점프 입력 전달
            onJumpCollTimeChange += jump.RefreshCoolTime; // 점프 쿨타임 전달
            onDie += jump.Stop; // 플레이어 죽으면 정지
        }
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
        if (isClear)
            return; 

        JumpCoolRemain -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            GroundCount++;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCount--;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if(platform != null)
        {
            platform.onMove += OnRideMovingObject; // 플랫폼 트리거에 들어갔을 때 연결 
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove -= OnRideMovingObject; // 플랫폼 트리거에서 나왔을 때 연결 해제
        }
    }

    /// <summary>
    /// 움직이는 물체에 탑승했을 때 연결될 함수
    /// </summary>
    /// <param name="delta"></param>
    void OnRideMovingObject(Vector3 delta)
    {
        rigid.MovePosition(rigid.position + delta);
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
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDirection * transform.forward);
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
            JumpCoolRemain = jumpCooltime; // 점프 쿨타임 초기화
            //GroundCount = 0; // 모든 땅에서 떨어짐
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

            GameManager.Instance.GameOver();
            isAlive = false;
        }
    }

    /// <summary>
    /// 이독 속도 증감용 함수
    /// </summary>
    /// <param name="ratdio">증감 비율</param>
    public void SetSpeedModifier(float ratdio = 1.0f)
    {
        currentMoveSpeed = moveSpeed * ratdio;
    }

    /// <summary>
    /// 원래 기준속도로 복구하는 함수
    /// </summary>
    public void RestoreMoveSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }

    void GameClear()
    {
        isClear = true;
    }
}