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

    void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        // ���� ������ ���� inputDirection �������� �ʴ� speed ��ŭ �̵�
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentSpeed * inputDirection);
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
}