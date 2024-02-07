using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Door : TestBase
{
    public GameObject door;
    public TextMeshPro showKey; // 3d ����
    public DoorSwitch test_switch;

    // ����
    // 1. �̴��̹� ������ �����(Door_Sliding_Auto); done
    // 2. ���� ���⿡���� ������ �����̹� ����� (����Ʈ �ʿ��� ������ ���� ������) , �ڵ��

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Vector3 cameraForward = Camera.main.transform.forward;

        float angle = Vector3.SignedAngle(door.transform.forward, cameraForward, Vector3.up);
        Debug.Log($"{angle}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        IInteracable inter = door.GetComponent<IInteracable>();
        inter.Use();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        IInteracable inter = test_switch.GetComponent<IInteracable>();
        inter.Use();
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Die();
    }
}
