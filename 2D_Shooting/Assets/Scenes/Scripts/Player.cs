using System;
using System.Collections;
using System.Collections.Generic;
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
    readonly int inputY_string = Animator.StringToHash("inputY");

    public GameObject Bullet;

    private Vector2 inputDir = Vector2.zero;
    public float speed = 5f;
    public float boostSpeed = 1.5f;

    void Awake()
    {
        action = new Player_InputAction(); // new inputAction

        // GetComponent �Լ��� ������ -> �ѹ��� ����ǰ� Awake�� ����
        anim = GetComponent<Animator>(); // �� ��ũ��Ʈ�� ����ִ� ���� ������Ʈ���� ������Ʈ�� ã�Ƽ� ����
    }

    void OnEnable()
    {
        action.Player.Enable(); // Player action map enable
        action.Player.Fire.performed += OnFire; // performed : Key Down , (Delegate valuable)
                                                // add onFire Funtion to Fire at Player action map
        action.Player.Fire.canceled += OnFire;  // canceled : Key up
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
        action.Player.Fire.canceled -= OnFire;
        action.Player.Fire.performed -= OnFire;
        action.Player.Disable(); // disable
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


    /// <summary>
    /// Fire �׼��� �ߵ������� �����ų �Լ�
    /// </summary>
    /// <param name="�Է°��� ������ ���ִ� ����ü ����"></param>

    private void OnFire(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            Debug.Log("OnFire : Key Down");
            Instantiate(Bullet, transform.position, Quaternion.identity);
        }

        //if(context.canceled)
    }

    void Update()
    {
/*        transform.position += new Vector3(inputDir.x * speed,
                                          inputDir.y * speed, 0);*/

        transform.Translate(inputDir * speed * Time.deltaTime); // 1�ʴ� speed��ŭ inputDir�������� ��������
        // Time.deltaTime = �����Ӱ��� �ð� ����(������)
    }
}