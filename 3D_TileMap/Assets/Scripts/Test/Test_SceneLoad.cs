using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_SceneLoad : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("LoadSampleScene"); // Synchronous : �� �ڵ尡 ������ ���� �ڵ尡 ����ȴ�.
    }
}
