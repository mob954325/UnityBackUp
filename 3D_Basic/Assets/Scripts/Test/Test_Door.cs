using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Door : TestBase
{
    public GameObject door;
    public TextMeshPro showKey; // 3d 글자
    public DoorSwitch test_switch;

    // ㅅㅅ
    // 1. 미닫이문 프리팹 만들기(Door_Sliding_Auto); done
    // 2. 한쪽 방향에서만 열리는 여닫이문 만들기 (프론트 쪽에서 서있을 때만 열리기) , 코드로

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
