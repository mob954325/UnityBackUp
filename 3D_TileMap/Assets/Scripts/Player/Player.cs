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

    /// <summary>
    /// 플레이어가 현재 위치하고 있는 맵의 그리드
    /// </summary>
    Vector2Int currentMap;

    /// <summary>
    /// CurrentMap에 값을 설정할 때 변경이 있으면 델리게이트를 실행해서 알리는 프로퍼티
    /// </summary>
    Vector2Int CurrentMap
    {
        get => currentMap;
        set
        {
            if(value != currentMap)
            {
                currentMap = value;
                onMapChange?.Invoke(currentMap);    // 맵 변경을 알림
            }
        }
    }

    /// <summary>
    /// 플레이어가 있는 맵이 변경되면 실행되는 델리게이트
    /// </summary>
    public Action<Vector2Int> onMapChange;

    /// <summary>
    /// 월드 매니저
    /// </summary>
    WorldManager world;

    /// <summary>
    /// 플레이어의 최대 수명
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// 플레이어의 현재 수명
    /// </summary>
    float lifeTime;

    /// <summary>
    /// 수명을 확인하고 변경되었을 때 처리를 하는 프로퍼티
    /// </summary>
    float LifeTime
    {
        get => lifeTime;
        set
        {
            if (lifeTime != value)
            {
                lifeTime = value; // 값 설정
                if(isAlive && lifeTime < 0.001f) // 살아 있고 수명이 0 미만이면 사망
                {
                    Die();
                }
                else
                {
                    // 살아 있으면 일반 처리
                    lifeTime = Mathf.Clamp(lifeTime, 0.0f, maxLifeTime);    // 일정 범위를 벗어나지 않게 만들기
                    onLifeTimeChange?.Invoke(lifeTime/maxLifeTime);         // 수명이 변경되었음을 알림
                }
            }
        }
    }

    /// <summary>
    /// 전체 플레이 시간
    /// </summary>
    float totalPlayTime = 0.0f;

    // 실습
    // 1. 시작하면 플레이어의 수명이 최대 수명으로 변경
    // 2. 시작이 지날 수록 플레이어의 수명이 감소 (초당 1)
    // 3. 수명의 변경 변화가 LIfeTimeGauge UI에 반영되어야 한다.


    /// <summary>
    /// 플레이어의 수명이 변경되었을 때 실행 될 델리게이트(float : 수명의 비율)
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// 플레이어가 죽었음을 알리는 델리게이트(float : 전체 플레이시간, int : 킬 카운트)
    /// </summary>
    public Action<float, int> onDie;

    /// <summary>
    /// 살아있는지 확인하는 변수
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// 잡은 슬라임 수
    /// </summary>
    int killCount = -1;

    int KillCount
    {
        get => killCount;
        set
        {
            if(killCount != value)
            {
                killCount = value;
                onKillCountChange?.Invoke(killCount);
            }
        }
    }

    public Action<int> onKillCountChange;

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

    void Start()
    {
        lifeTime = maxLifeTime; // 생명 초기화
        world = GameManager.Instance.World;

        KillCount = 0;
    }

    void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
        totalPlayTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        // 물리 프레임 마다 inputDirection 방향으로 초당 speed 만큼 이동
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);

        CurrentMap = world.WorldToGrid(rigid.position); // 플레이어가 있는 맵 설정
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

    /// <summary>
    /// 몬스터를 잡았을 때 실행할 함수
    /// </summary>
    /// <param name="bonus">몬스터 처치 보너스</param>
    public void MosterKill(float bonus)
    {
        LifeTime += bonus;
        KillCount++;
    }

    /// <summary>
    /// 플레이어가 죽었을 때 처리할 일을 처리하는 함수
    /// </summary>
    private void Die()
    {
        isAlive = false;                            // 죽었다고 표시
        LifeTime = 0f;                              // 수명을 0으로 정리
        onLifeTimeChange?.Invoke(0);                // 수명 변화를 알리기
        inputAction.Player.Disable();               // 플레이어 입력 정지
        onDie?.Invoke(totalPlayTime, KillCount);    // 사망 알리기
    }

#if UNITY_EDITOR
    public void Test_Die()
    {
        LifeTime = -1f;
    }
#endif

}