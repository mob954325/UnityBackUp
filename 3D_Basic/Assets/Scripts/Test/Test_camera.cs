using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_camera : TestBase
{
    public CinemachineVirtualCamera[] vcams;

    private void Start()
    {
        if(vcams == null)
        {
            vcams = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None);
        }
    }
    Player player;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Die();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        vcams[0].Priority = 100;
        vcams[1].Priority = 10;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        vcams[0].Priority = 10;
        vcams[1].Priority = 100;
    }
}
