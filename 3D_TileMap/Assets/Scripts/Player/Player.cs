using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ���� �� ���ݳ� ���� targetList
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
    /// ���� ������ ��ȿ�� �������� Ȯ���ϴ� ����
    /// </summary>
    bool isAttackValid = false;

    // Input values
    /// <summary>
    /// ���� �Էµ� �̵� ����
    /// </summary>
    Vector2 inputDirection = Vector2.zero;

    /// <summary>
    /// ���� �����̰� �ִ��� Ȯ���ϴ� bool��(�����̸� true)
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// ���� �����ִ� ���� ��Ÿ��
    /// </summary>
    float currentAttackCoolTime = 0.0f;

    /// <summary>
    /// ������ �غ����� Ȯ���ϴ� ���� ( true�� ���� ���� )
    /// </summary>
    bool isAttackReady => currentAttackCoolTime < 0.0f;

    // �ִϸ����Ϳ� �ؽð�
    readonly int InputX_Hash = Animator.StringToHash("InputX");
    readonly int InputY_Hash = Animator.StringToHash("InputY");
    readonly int IsMove_Hash = Animator.StringToHash("isMove");
    readonly int Attack_Hash = Animator.StringToHash("Attack");

    /// <summary>
    /// �÷��̾ ���� ��ġ�ϰ� �ִ� ���� �׸���
    /// </summary>
    Vector2Int currentMap;

    /// <summary>
    /// CurrentMap�� ���� ������ �� ������ ������ ��������Ʈ�� �����ؼ� �˸��� ������Ƽ
    /// </summary>
    Vector2Int CurrentMap
    {
        get => currentMap;
        set
        {
            if(value != currentMap)
            {
                currentMap = value;
                onMapChange?.Invoke(currentMap);    // �� ������ �˸�
            }
        }
    }

    /// <summary>
    /// �÷��̾ �ִ� ���� ����Ǹ� ����Ǵ� ��������Ʈ
    /// </summary>
    public Action<Vector2Int> onMapChange;

    /// <summary>
    /// ���� �Ŵ���
    /// </summary>
    WorldManager world;

    /// <summary>
    /// �÷��̾��� �ִ� ����
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// �÷��̾��� ���� ����
    /// </summary>
    float lifeTime;

    /// <summary>
    /// ������ Ȯ���ϰ� ����Ǿ��� �� ó���� �ϴ� ������Ƽ
    /// </summary>
    float LifeTime
    {
        get => lifeTime;
        set
        {
            if (lifeTime != value)
            {
                lifeTime = value; // �� ����
                if(isAlive && lifeTime < 0.001f) // ��� �ְ� ������ 0 �̸��̸� ���
                {
                    Die();
                }
                else
                {
                    // ��� ������ �Ϲ� ó��
                    lifeTime = Mathf.Clamp(lifeTime, 0.0f, maxLifeTime);    // ���� ������ ����� �ʰ� �����
                    onLifeTimeChange?.Invoke(lifeTime/maxLifeTime);         // ������ ����Ǿ����� �˸�
                }
            }
        }
    }

    /// <summary>
    /// ��ü �÷��� �ð�
    /// </summary>
    float totalPlayTime = 0.0f;

    // �ǽ�
    // 1. �����ϸ� �÷��̾��� ������ �ִ� �������� ����
    // 2. ������ ���� ���� �÷��̾��� ������ ���� (�ʴ� 1)
    // 3. ������ ���� ��ȭ�� LIfeTimeGauge UI�� �ݿ��Ǿ�� �Ѵ�.


    /// <summary>
    /// �÷��̾��� ������ ����Ǿ��� �� ���� �� ��������Ʈ(float : ������ ����)
    /// </summary>
    public Action<float> onLifeTimeChange;

    /// <summary>
    /// �÷��̾ �׾����� �˸��� ��������Ʈ(float : ��ü �÷��̽ð�, int : ų ī��Ʈ)
    /// </summary>
    public Action<float, int> onDie;

    /// <summary>
    /// ����ִ��� Ȯ���ϴ� ����
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// ���� ������ ��
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

        attackTargetList = new List<Slime>(4); // ũ�� 4�� ����
        AttackSensor sensor = attackAxisObj.GetComponentInChildren<AttackSensor>();

        sensor.onSlimeEnter += (slime) =>   // ���� ���� �ȿ� ������
        {
            if(isAttackValid)
            {
                //������ ��ȿ�ϸ� 
                slime.Die(); // ��� ���̱�
            }
            else
            {
                attackTargetList.Add(slime);     // ����Ʈ �߰�
            }
            slime.ShowOutline();                 // �ƿ����� �׸���
        };
        sensor.onSlimeExit += (slime) =>         // ���� ������ �����
        { 
            attackTargetList.Remove(slime);      // ����Ʈ ����
            slime.ShowOutline(false);            // �ƿ����� ����
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
        lifeTime = maxLifeTime; // ���� �ʱ�ȭ
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
        // ���� ������ ���� inputDirection �������� �ʴ� speed ��ŭ �̵�
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);

        CurrentMap = world.WorldToGrid(rigid.position); // �÷��̾ �ִ� �� ����
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // �Է°� �ޱ�
        inputDirection = context.ReadValue<Vector2>();

        // �ִϸ��̼� ����
        animtor.SetFloat(InputX_Hash, inputDirection.x);
        animtor.SetFloat(InputY_Hash, inputDirection.y);
        isMove = true;
        animtor.SetBool(IsMove_Hash, isMove);

        // ���� ���� ȸ����Ű��
        AttackSensorRotate(inputDirection);
    }

    private void OnStop(InputAction.CallbackContext _)
    {
        // �Է¹��� 0
        inputDirection = Vector2.zero;

        // InputX�� InputY�� ���� �ʴ� ������ 
        // Idle �ִϸ��̼��� ������ �̵� �������� ����ϱ� ���ؼ�

        isMove = false; // ����
        animtor.SetBool(IsMove_Hash, isMove);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (isAttackReady) // ���� ��Ÿ���� �� �Ǿ����� 
        {
            animtor.SetTrigger(Attack_Hash);        // �ִϸ��̼� ����
            currentAttackCoolTime = attackCoolTime; // ��Ÿ�� �ʱ�ȭ
            currentSpeed = 0f;                      // �̵� ����
            isAttackValid = false;                  // isAttackValid ��Ȱ��ȭ
        }
    }

    /// <summary>
    /// �̵� �ӵ��� ���A�� �ǵ����� �Լ�
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }

    /// <summary>
    /// �Է� ���⿡ ���� AttackSensor�� ȸ�� ��Ű�� �Լ�
    /// </summary>
    /// <param name="direction"></param>
    void AttackSensorRotate(Vector2 direction)
    {
        // attackSensorAxis

        if(direction.y < 0)
        {
            attackAxisObj.rotation = quaternion.identity; // �Ʒ�
        } 
        else if(direction.y > 0)
        {
            attackAxisObj.rotation = Quaternion.Euler(0, 0, 180); // ��
        }
        else if(direction.x < 0)
        { 
            attackAxisObj.rotation = Quaternion.Euler(0, 0, -90); // ����
        }
        else if(direction.x > 0)
        {
            attackAxisObj.rotation = Quaternion.Euler(0, 0, 90); // ������
        }
        else
        {
            attackAxisObj.rotation = quaternion.identity;
        }
    }
    // �� �� ��� �Լ���

    /// <summary>
    /// ���� �ִϸ��̼� ���� �߿� ������ ��ȿ������ �ִϸ��̼� �̺�Ʈ ����
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
    /// ���� �ִϸ��̼� ���� �߿� ������ ��ȿ���� �ʰ� �Ǹ� �ִϸ��̼� �̺�Ʈ ����
    /// </summary>
    void AttackNotValid()
    {
        Debug.Log("attackNotValid"); 
        isAttackValid = false;
    }

    /// <summary>
    /// ���͸� ����� �� ������ �Լ�
    /// </summary>
    /// <param name="bonus">���� óġ ���ʽ�</param>
    public void MosterKill(float bonus)
    {
        LifeTime += bonus;
        KillCount++;
    }

    /// <summary>
    /// �÷��̾ �׾��� �� ó���� ���� ó���ϴ� �Լ�
    /// </summary>
    private void Die()
    {
        isAlive = false;                            // �׾��ٰ� ǥ��
        LifeTime = 0f;                              // ������ 0���� ����
        onLifeTimeChange?.Invoke(0);                // ���� ��ȭ�� �˸���
        inputAction.Player.Disable();               // �÷��̾� �Է� ����
        onDie?.Invoke(totalPlayTime, KillCount);    // ��� �˸���
    }

#if UNITY_EDITOR
    public void Test_Die()
    {
        LifeTime = -1f;
    }
#endif

}