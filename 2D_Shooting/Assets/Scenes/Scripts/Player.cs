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
    readonly int inputY_string = Animator.StringToHash("inputY");

    public GameObject Bullet;

    private Vector2 inputDir = Vector2.zero;
    public float speed = 5f;
    public float boostSpeed = 1.5f;

    void Awake()
    {
        action = new Player_InputAction(); // new inputAction

        // GetComponent 함수는 느리다 -> 한번만 실행되게 Awake에 실행
        anim = GetComponent<Animator>(); // 이 스크립트가 들어있는 게임 오브젝트에서 컴포넌트를 찾아서 저장
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


    /// <summary>
    /// Fire 액션이 발동했을때 실행시킬 함수
    /// </summary>
    /// <param name="입력관련 정보가 들어가있는 구조체 변수"></param>

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

        transform.Translate(inputDir * speed * Time.deltaTime); // 1초당 speed만큼 inputDir방향으로 움직여라
        // Time.deltaTime = 프레임간의 시간 간격(가변적)
    }
}