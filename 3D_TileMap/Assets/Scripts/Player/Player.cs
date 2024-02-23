using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 현재 내 공격내 들어온 targetList
    /// </summary>
    List<Slime> attackTargetList;

    // Component
    PlayerInputAction inputAction;
    Rigidbody2D rigid;
    Animator animtor;
    AttackSensor attackSensor;

    Transform attackAxisObj;

    // stats
    public float currentSpeed = 3.0f;
    public float speed = 3.0f;

    /// <summary>
    /// 지금 공격이 유효한 상태인지 확인하는 변수
    /// </summary>
    bool isAttackValid = false;

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

        attackAxisObj = transform.GetChild(0);

        attackTargetList = new List<Slime>(4); // 크기 4로 지정
        AttackSensor sensor = attackAxisObj.GetComponentInChildren<AttackSensor>();

        sensor.onSlimeEnter += (slime) =>   // 적이 센서 안에 들어오면
        {
            if(isAttackValid)
            {
                //공격이 유효하면 
                slime.Die(); // 즉시 죽이기
            }
            else
            {
                attackTargetList.Add(slime);     // 리스트 추가
            }
            slime.ShowOutline();                 // 아웃라인 그리기
        };
        sensor.onSlimeExit += (slime) =>         // 적이 센서에 벗어나면
        { 
            attackTargetList.Remove(slime);      // 리스트 제거
            slime.ShowOutline(false);            // 아웃라인 제거
        };
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

        // 공격 범위 회전시키기
        AttackSensorRotate(inputDirection);
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
            currentSpeed = 0f;                      // 이동 정지
            isAttackValid = false;                  // isAttackValid 비활성화
        }
    }

    /// <summary>
    /// 이동 속도를 원랟로 되돌리는 함수
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }

    /// <summary>
    /// 입력 방향에 따라 AttackSensor를 회전 시키는 함수
    /// </summary>
    /// <param name="direction"></param>
    void AttackSensorRotate(Vector2 direction)
    {
        // attackSensorAxis

        if(direction.y < 0)
        {
            attackAxisObj.rotation = quaternion.identity; // 아래
        } 
        else if(direction.y > 0)
        {
            attackAxisObj.rotation = Quaternion.Euler(0, 0, 180); // 위
        }
        else if(direction.x < 0)
        { 
            attackAxisObj.rotation = Quaternion.Euler(0, 0, -90); // 왼쪽
        }
        else if(direction.x > 0)
        {
            attackAxisObj.rotation = Quaternion.Euler(0, 0, 90); // 오른쪽
        }
        else
        {
            attackAxisObj.rotation = quaternion.identity;
        }
    }
    // 그 외 기능 함수들

    /// <summary>
    /// 공격 애니메이션 진행 중에 공격이 유효해지면 애니메이션 이벤트 실행
    /// </summary>
    void AttackValid()
    {
        isAttackValid = true;
        foreach (var slime in attackTargetList)
        {
            slime.Die();
        }
        attackTargetList.Clear();
    }

    /// <summary>
    /// 공격 애니메이션 진행 중에 공격이 유효하지 않게 되면 애니메이션 이벤트 실행
    /// </summary>
    void AttackNotValid()
    {
        Debug.Log("attackNotValid"); 
        isAttackValid = false;
    }
}