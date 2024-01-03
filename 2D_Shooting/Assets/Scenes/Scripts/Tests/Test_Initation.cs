using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Initation : Test_base
{
    public GameObject prefab;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Debug.Log("1���� �������ϴ�.");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // �Լ� �����ε� : ���� �Լ��� �ٸ� �Ķ���͸� ���� �Լ�
        Instantiate(prefab); // prefab ���� : 0,0,0

        // ������ǥ
        // ������ǥ
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // �����ո���� : ���ӿ�����Ʈ�� 5,0,0�� �����
        Instantiate(prefab, new Vector3(5, 0, 0), Quaternion.identity);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Instantiate(prefab, this.transform);// �θ���� ����
    }
}
