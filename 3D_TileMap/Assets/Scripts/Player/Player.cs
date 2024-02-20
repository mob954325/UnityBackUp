using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Component
    PlayerInputAction inputAction;
    Rigidbody2D rigid;
    Animator animtor;

    // stats
    public float currentSpeed = 3.0f;
    public float speed = 3.0f;

    // Input values
    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 inputDirection = Vector2.zero;
    /// <summary>
    /// 지금 움직이고 있는지 확인하는 bool값(움직이면 true)
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 1.0f;
    /// <summary>
    /// 현재 남아있는 공격 쿨타임
    /// </summary>
    float currentAttackCoolTime = 0.0f;
    /// <summary>
    /// 공격이 준비됬는지 확인하는 변수 ( true면 공격 가능 )
    /// </summary>
    bool isAttackReady => currentAttackCoolTime < 0.0f;

    // 애니메이터용 해시값
    readonly int InputX_Hash = Animator.StringToHash("InputX");
    readonly int InputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("isMove");
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    void Awake()
    {
        inputAction = new PlayerInputAction();

        rigid = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();
    }

    void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnStop;
        inputAction.Player.Attack.performed += OnAttack;
    }

    void OnDisable()
    {
        inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.Move.canceled -= OnStop;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Disable();
    }

    void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        // 물리 프레임 마다 inputDirection 방향으로 초당 speed 만큼 이동
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 입력값 받기
        inputDirection = context.ReadValue<Vector2>();

        // 애니메이션 조정
        animtor.SetFloat(InputX_Hash, inputDirection.x);
        animtor.SetFloat(InputY_Hash, inputDirection.y);
        isMove = true;
        animtor.SetBool(IsMove_Hash, isMove);
    }

    private void OnStop(InputAction.CallbackContext _)
    {
        // 입력방향 0
        inputDirection = Vector2.zero;

        // InputX과 InputY를 받지 않는 이유는 
        // Idle 애니메이션을 마지막 이동 방향으로 재생하기 위해서

        isMove = false; // 정지
        animtor.SetBool(IsMove_Hash, isMove);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (isAttackReady) // 공격 쿨타임이 다 되었으면 
        {
            animtor.SetTrigger(Attack_Hash);        // 애니메이션 실행
            currentAttackCoolTime = attackCoolTime; // 쿨타임 초기화
            currentSpeed = 0f;
        }
    }

    /// <summary>
    /// 이동 속도를 원랟로 되돌리는 함수
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }

    // 그 외 기능 함수들
}