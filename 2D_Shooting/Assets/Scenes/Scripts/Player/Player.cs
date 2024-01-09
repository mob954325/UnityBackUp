using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Animator))] // �ݵ�� Ư�� ������Ʈ�� �ʿ��� ��쿡 �߰� -> ������ �߰���

public class Player : MonoBehaviour
{
    //[SerializeField]private float speed = 0.01f; -> unity������ public ���� : ���ɹ���
    //[Range(0.1f,1f)] : ��ũ�� ����
    Player_InputAction action;
    Animator anim;
    Rigidbody2D rigid;

    readonly int inputY_string = Animator.StringToHash("inputY");

    public GameObject bullet;
    public GameObject fireFlash;
    public Transform fireTransform;

    private Vector2 inputDir = Vector2.zero;
    public float speed = 5f;
    public float boostSpeed = 1.5f;
    /// <summary>
    /// ���� �ð� ����
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// �÷����� ��ٸ��� �ð�
    /// </summary>
    WaitForSeconds flashWait;

    /// <summary>
    /// ���縦 ������ �ڷ�ƾ
    /// </summary>
    IEnumerator fireCoroutine;

    /// <summary>
    /// Player score
    /// </summary>
    int score = 0;


    /// <summary>
    /// ���� Ȯ��
    /// </summary>
    public int Score
    {
        get => score;

        private set
        {
            if(score != value)
            {
                score = Math.Min(value, 99999); // �ִ����� 99999

                onScoreChange?.Invoke(score); // �� ���� ����Ʈ�� ������ ����� ��� ��� ����� ������ �˸�
            }
        }
    }

    /// <summary>
    /// �Լ��� ����Ǿ��� �� �˸��� ��������Ʈ(�Ķ���� : ����� ����)
    /// </summary>
    public Action<int> onScoreChange;

    public delegate int ScoreDelegate(int enemyScore);
    public ScoreDelegate PlayerGetScore;

    void Awake()
    {
        fireFlash.SetActive(false); // falsh disable
        action = new Player_InputAction(); // new inputAction

        // ���� ������Ʈ ã�� ���
        //GameObject.Find("FirePosition"); , //������Ʈ �̸����� ã��
        //GameObject.FindAnyObjectByType<Transform>(); , //������Ʈ Ÿ������ ã��
        //GameObject.FindFirstObjectByType<Transform>(); //Ư�� ������Ʈ�� ������ �ִ� ù��° ���� ������Ʈ
        //GameObject.FindGameObjectWithTag("player") //�±׷� ã��
        //GameObject.FindGameObjectsWithTag("Player"); //�±׸� ���� ��� ������Ʈ�� ��������

        // GetComponent �Լ��� ������ -> �ѹ��� ����ǰ� Awake�� ����
        anim = GetComponent<Animator>(); // �� ��ũ��Ʈ�� ����ִ� ���� ������Ʈ���� ������Ʈ�� ã�Ƽ� ����
        rigid = GetComponent<Rigidbody2D>();    

        fireTransform = transform.GetChild(0);  // .GetChild(int n); : ���� ���� ������Ʈ�� �ڽ� ã��
        fireFlash = transform.GetChild(1).gameObject; // ���� ���� ������Ʈ�� 2��° �ڽ�ã�Ƽ� firFlash�� ����
        flashWait = new WaitForSeconds(0.1f);
        fireCoroutine = FireCoroutine();
    }

    void OnEnable()
    {
        action.Player.Enable(); // Player action map enable
        action.Player.Fire.performed += OnFireStart; // performed : Key Down , (Delegate valuable)
                                                     // add onFire Funtion to Fire at Player action map
        action.Player.Fire.canceled += OnFireEnd;     // canceled : Key up
        action.Player.Boost.performed += OnBoost;
        action.Player.Boost.canceled += OnBoost;
        action.Player.Move.performed += OnMove;
        action.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        action.Player.Move.canceled -= OnMove;
        action.Player.Move.performed -= OnMove;
        action.Player.Boost.canceled -= OnBoost;
        action.Player.Move.performed -= OnBoost;
        action.Player.Fire.canceled -= OnFireStart;
        action.Player.Fire.performed -= OnFireEnd;
        action.Player.Disable(); // disable
    }

    void LateUpdate()
    {
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        

        anim.SetFloat(inputY_string, inputDir.y); // inputY parameter�� inputDir.y�� ����
        //Vector2 dir = context.ReadValue<Vector2>();

        //Debug.Log($"{dir}");



        /*transform.position += (Vector3)dir;*/
    }
    private void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnBoost : Key Down");
            speed *= boostSpeed;
        }

        if (context.canceled)
        {
            Debug.Log("OnBoost : Key Up");
            speed /= boostSpeed;
        }
    }
    private void OnFireStart(InputAction.CallbackContext _)
    {
        /*if(context.performed)
        {
             Fire(fireTransform.position);
        }*/

        //if(context.canceled)

        StartCoroutine(fireCoroutine); // ���� ����
    }

    private void OnFireEnd(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine); // ���� ����
    }

    IEnumerator FireCoroutine()
    {
        while(true)
        {
            Fire(fireTransform.position);
            yield return new WaitForSeconds(fireInterval);
        }
    }

    /// <summary>
    /// �Ѿ��� �߻��ϴ� �Լ�
    /// </summary>
    /// <param name="position">�Ѿ��� ��ġ</param>
    /// <param name="angle">�Ѿ��� ����</param>
    void Fire(Vector3 position, float angle = 0.0f)
    {
        // fireFlash effect
        Instantiate(bullet, position, Quaternion.identity);
        StartCoroutine(Co_fireFlashEffect());
    }

    void Update()
    {
/*        transform.position += new Vector3(inputDir.x * speed,
                                          inputDir.y * speed, 0);*/

        // transform.Translate(inputDir * speed * Time.deltaTime); // 1�ʴ� speed��ŭ inputDir�������� ��������
        // Time.deltaTime = �����Ӱ��� �ð� ����(������)
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + Time.deltaTime * speed * inputDir);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹�� �������� �� ����
        Debug.Log("OnCollisionEnter2D");

        Debug.Log($"Collider2D : {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Enemy")) // collision�� ���� ������Ʈ�� "Enemy"��� �±׸� �������� Ȯ��
        {
            Destroy(collision.gameObject); // �浹�� ����� ����
        }
    }

    IEnumerator Co_fireFlashEffect()
    {
        fireFlash.SetActive(true);
        yield return flashWait; // flahsWait(0.1f)��ŭ ��ٸ���
        fireFlash.SetActive(false);
    }


    /// <summary>
    /// add Score Func
    /// </summary>
    /// <param name="getScore">get score</param>
    public void AddScore(int getScore)
    {
        Score += getScore;
    }
}