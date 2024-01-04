using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Animator))] // 반드시 특정 컴포넌트가 필요한 경우에 추가 -> 없으면 추가함

public class Player : MonoBehaviour
{
    //[SerializeField]private float speed = 0.01f; -> unity에서는 public 권장 : 성능문제
    //[Range(0.1f,1f)] : 스크롤 조절
    Player_InputAction action;
    Animator anim;
    Rigidbody2D rigid;

    readonly int inputY_string = Animator.StringToHash("inputY");

    public GameObject bullet;
    public GameObject flash;
    public Transform fireTransform;

    private Vector2 inputDir = Vector2.zero;
    public float speed = 5f;
    public float boostSpeed = 1.5f;

    void Awake()
    {
        flash.SetActive(false); // falsh disable
        action = new Player_InputAction(); // new inputAction

        // 게임 오브젝트 찾는 방법
        //GameObject.Find("FirePosition"); , //오브젝트 이름으로 찾기
        //GameObject.FindAnyObjectByType<Transform>(); , //오브젝트 타입으로 찾기
        //GameObject.FindFirstObjectByType<Transform>(); //특정 컴포넌트를 가지고 있는 첫번째 게임 오브젝트
        //GameObject.FindGameObjectWithTag("player") //태그로 찾기
        //GameObject.FindGameObjectsWithTag("Player"); //태그를 가진 모든 오브젝트를 가져오기

        // GetComponent 함수는 느리다 -> 한번만 실행되게 Awake에 실행
        anim = GetComponent<Animator>(); // 이 스크립트가 들어있는 게임 오브젝트에서 컴포넌트를 찾아서 저장
        rigid = GetComponent<Rigidbody2D>();    

        fireTransform = transform.GetChild(0);  // .GetChild(int n); : 현재 게임 오브젝트의 자식 찾기

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
        

        anim.SetFloat(inputY_string, inputDir.y); // inputY parameter에 inputDir.y를 보냄
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
    private void OnFire(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            StartCoroutine(Co_flashEffect());
            Instantiate(bullet, fireTransform.position, Quaternion.identity);
        }

        //if(context.canceled)
    }

    void Update()
    {
/*        transform.position += new Vector3(inputDir.x * speed,
                                          inputDir.y * speed, 0);*/

        // transform.Translate(inputDir * speed * Time.deltaTime); // 1초당 speed만큼 inputDir방향으로 움직여라
        // Time.deltaTime = 프레임간의 시간 간격(가변적)
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + Time.deltaTime * speed * inputDir);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌이 시작했을 때 실행
        Debug.Log("OnCollisionEnter2D");

        Debug.Log($"Collider2D : {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Enemy")) // collision의 게임 오브젝트가 "Enemy"라는 태그를 가지는지 확인
        {
            Destroy(collision.gameObject); // 충돌한 대상을 제거
        }
    }

    IEnumerator Co_flashEffect()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
    }
}