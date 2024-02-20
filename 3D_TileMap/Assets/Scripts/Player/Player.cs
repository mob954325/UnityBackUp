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
            currentSpeed = 0f;
        }
    }

    /// <summary>
    /// �̵� �ӵ��� ���A�� �ǵ����� �Լ�
    /// </summary>
    public void RestoreSpeed()
    {
        currentSpeed = speed;
    }

    // �� �� ��� �Լ���
}