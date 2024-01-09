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
    // Delegate() : �Լ��� ������ �� �ִ� ���� Ÿ��

    public delegate void TestDelegate1(); // create deletage type(�Ķ����, ���ϰ��� ���� �Լ��� ���� �� �� �ִ�.)
                                         // name : TestDelegate

    TestDelegate1 aaa; // TestDelegateŸ������ ������ ������ �� �ִ� aaa��� ������ ����

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

    public delegate int TestDelegate2(int a); // �� ��������Ʈ�� �Ķ���Ͱ� 2�� int, float�� ������ int���� �Ѵ�.
    TestDelegate2 bbb;


    int TestRun4(int a, float b)
    {
        return a + (int)b;
    }

    void Start()
    {
        bbb = TestRun5;

        aaa = TestRun1; // ������ ��ϵ� �ִ� �Լ��� �� ������ TestRun�Լ��� ���
        aaa += TestRun2; // ������ ��ϵ� �Լ� �ڿ� TestRun �߰�
        aaa = TestRun3 + aaa; // aaa�� �տ� TestRun �߰�

    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        aaa(); // aaa delegate ����
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        aaa = null;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        base.OnTest3(context);

        aaa?.Invoke(); // ? null�̸� �������� �ƴϸ� ����

        // nullable Ÿ��(null���� ���� �� �ִ� Ÿ��)
        //int? i;
        //i = null;
        //i = 30;
    }

    Action ccc; // �Ķ���� ���� ���ϰ� ���� �Լ��� ������ �� �ִ� delegate
    Action<int> ddd; // �Ķ���ͷ� int �ϳ� ����ϰ� ���ϰ� ���� �Լ��� ������ �� �ִ� ��������Ʈ
    Action<int, int> eee;

    Func<int> f; // ���� Ÿ���� int �� �Լ��� ������ �� �ִ� ��������Ʈ
    Func<int,float> g; // �Ķ���Ͱ� int�ϳ��� ����Ÿ���� float�� �Լ��� ������ �� �ִ� delegate
    UnityEvent u1;

    void testunitydel()
    {

    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        u1.AddListener(testunitydel);
    }
}