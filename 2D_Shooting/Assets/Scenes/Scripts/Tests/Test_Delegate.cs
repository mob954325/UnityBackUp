using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Test_Delegate : Test_base
{
    // Delegate() : 함수를 저장할 수 있는 변수 타입

    public delegate void TestDelegate1(); // create deletage type(파라미터, 리턴값이 없는 함수만 저장 할 수 있다.)
                                         // name : TestDelegate

    TestDelegate1 aaa; // TestDelegate타입으로 변수를 저장할 수 있는 aaa라는 변수를 만듦

    void TestRun1()
    {
        Debug.Log("TestRun1");
    }

    void TestRun2()
    {
        Debug.Log("TestRun2");
    }
    void TestRun3()
    {
        Debug.Log("TestRun3");
    }

    int TestRun5(int a)
    {
        return 5;
    }

    public delegate int TestDelegate2(int a); // 이 델리게이트는 파라미터가 2개 int, float고 리턴을 int으로 한다.
    TestDelegate2 bbb;


    int TestRun4(int a, float b)
    {
        return a + (int)b;
    }

    void Start()
    {
        bbb = TestRun5;

        aaa = TestRun1; // 이전에 등록되 있는 함수는 다 버리고 TestRun함수만 등록
        aaa += TestRun2; // 이전에 등록된 함수 뒤에 TestRun 추가
        aaa = TestRun3 + aaa; // aaa맨 앞에 TestRun 추가

    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        aaa(); // aaa delegate 실행
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        aaa = null;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        base.OnTest3(context);

        aaa?.Invoke(); // ? null이면 하지말고 아니면 실행

        // nullable 타입(null값을 가질 수 있는 타입)
        //int? i;
        //i = null;
        //i = 30;
    }

    Action ccc; // 파라미터 없고 리턴값 없는 함수를 저장할 수 있는 delegate
    Action<int> ddd; // 파라미터로 int 하나 사용하고 리턴값 없는 함수를 저장할 수 있는 델리게이트
    Action<int, int> eee;

    Func<int> f; // 리턴 타입이 int 인 함수를 저장할 수 있는 델리게이트
    Func<int,float> g; // 파라미터가 int하나고 리턴타입이 float인 함수를 저장할 수 있는 delegate
    UnityEvent u1;

    void testunitydel()
    {

    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        u1.AddListener(testunitydel);
    }
}