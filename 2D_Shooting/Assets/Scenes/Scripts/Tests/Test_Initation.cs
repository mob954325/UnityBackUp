using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Initation : Test_base
{
    public GameObject prefab;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Debug.Log("1번을 눌렀습니다.");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 함수 오버로딩 : 같은 함수의 다른 파라미터를 가진 함수
        Instantiate(prefab); // prefab 생성 : 0,0,0

        // 로컬좌표
        // 월드좌표
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // 프리팹만들기 : 게임오브젝트를 5,0,0에 만들기
        Instantiate(prefab, new Vector3(5, 0, 0), Quaternion.identity);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Instantiate(prefab, this.transform);// 부모기준 생성
    }
}
