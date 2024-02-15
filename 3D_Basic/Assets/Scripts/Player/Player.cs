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
    /// ���� �̵� �ӵ�
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
    /// �����ϰ� �ִ� "Ground" �±� ������Ʈ�� ����Ȯ�� �� ������ ������Ƽ
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
    /// �����ϰ� �ִ� "Ground" �±� ������Ʈ�� ����
    /// </summary>
    int groundCount = 0;

    /// <summary>
    /// Jump CoolDown
    /// </summary>
    public float jumpCooltime = 5.0f;

    /// <summary>
    ///  ���� �ִ� ��Ÿ��
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
    /// �÷��̾��� ���� ����
    /// </summary>
    bool isAlive = true;

    /// <summary>
    /// ������ �������� Ȯ���ϴ� ������Ƽ(�������� �ƴϰ� ��Ÿ���� �� ������.)
    /// </summary>
    bool IsJumpAvailable => !InAir && (JumpCoolRemain < 0.0f) && isAlive;

    /// <summary>
    /// Animator Hash Values
    /// </summary>
    readonly int isMoveHash = Animator.StringToHash("isMove");
    readonly int UseHash = Animator.StringToHash("Use");
    readonly int DieHash = Animator.StringToHash("Die");

    /// <summary>
    /// �÷��̾��� ����� �˸��� ��������Ʈ
    /// </summary>
    public Action onDie;

    /// <summary>
    /// �������� ���� �÷��̾� ����
    /// </summary>
    public float startLifeTime = 10.0f;

    /// <summary>
    /// ���� �÷��̾� ����
    /// </summary>
    float lifeTime = 0.0f;

    /// <summary>
    /// �÷��̾��� ������ Ȯ���ϰ� �����ϱ� ���� ������Ƽ
    /// </summary>
    float LifeTime
    {
        get => lifeTime;
        set
        {
            lifeTime = value;
            if (lifeTime <= 0.0f)
            {
                lifeTime = 0.0f; // ������ �� �Ǿ����� 0.0���� ���� ���� �� ���ó��
                Die();
            }
            onLifeTimeChange?.Invoke(lifeTime / startLifeTime); // ���� ���� ������ �˸�
        }
    }

    bool isClear = false;

    /// <summary>
    /// ������ ����� �� ����� ��������Ʈ
    /// </summary>
    public Action<float> onLifeTimeChange;

    void Awake()
    {
        //playerAction = new PlayerInput();
        playerAction = new(); // ������ Ÿ���� �ָ��ϸ� �ȵ�
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

        // ���� ��ƽ �����ϱ�
        VirtualStick stick = GameManager.Instance.Stick;
        if(stick != null)
        {
            stick.OnMoveInput += (inputDelta) => SetInput(inputDelta, inputDelta.sqrMagnitude > 0.0025f);// �̵� �Է�����
            onDie += stick.Stop; // �÷��̾� ������ ����
        }

        VirtualButton jump = GameManager.Instance.JumpButton;
        if(jump != null)
        {
            jump.onClick += Jump; // ���� �Է� ����
            onJumpCollTimeChange += jump.RefreshCoolTime; // ���� ��Ÿ�� ����
            onDie += jump.Stop; // �÷��̾� ������ ����
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
            platform.onMove += OnRideMovingObject; // �÷��� Ʈ���ſ� ���� �� ���� 
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlatformBase platform = other.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.onMove -= OnRideMovingObject; // �÷��� Ʈ���ſ��� ������ �� ���� ����
        }
    }

    /// <summary>
    /// �����̴� ��ü�� ž������ �� ����� �Լ�
    /// </summary>
    /// <param name="delta"></param>
    void OnRideMovingObject(Vector3 delta)
    {
        rigid.MovePosition(rigid.position + delta);
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
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDirection * transform.forward);
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
            JumpCoolRemain = jumpCooltime; // ���� ��Ÿ�� �ʱ�ȭ
            //GroundCount = 0; // ��� ������ ������
        }
    }

    /// <summary>
    /// ���ó���� �Լ�
    /// </summary>
    public void Die()
    {
        if(isAlive)
        {
            Debug.Log("Player �׾���");

            animator.SetTrigger(DieHash);
            playerAction.Disable();

            Transform head = transform.GetChild(0); // Head Object
            rigid.constraints = RigidbodyConstraints.None; // ���� ����� ���� �����ϱ�
            rigid.AddForceAtPosition(-transform.forward, head.position, ForceMode.Impulse);
            rigid.AddTorque(transform.up * 1.5f, ForceMode.Impulse);

            onDie?.Invoke();

            // �״� �ִϸ��̼��� ���´�.
            // �� �̻� ������ �ȵǾ���Ѵ�.
            // �뱼�뱼 ������. (�ڷ� �Ѿ�鼭 y������ ������ �Դ´�.)
            // �׾��ٰ� ��ȣ������ => ��������Ʈ

            GameManager.Instance.GameOver();
            isAlive = false;
        }
    }

    /// <summary>
    /// �̵� �ӵ� ������ �Լ�
    /// </summary>
    /// <param name="ratdio">���� ����</param>
    public void SetSpeedModifier(float ratdio = 1.0f)
    {
        currentMoveSpeed = moveSpeed * ratdio;
    }

    /// <summary>
    /// ���� ���ؼӵ��� �����ϴ� �Լ�
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