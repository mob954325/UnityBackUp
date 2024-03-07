using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_SeemAdditveLoad : TestBase
{
    [Range(0, 2)]
    public int targetX = 0;

    [Range(0, 2)]
    public int targetY = 0;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // LoadSceneMode.Single : 씬에 있는 모든 걸 제거후 로드
        // LoadSceneMode.Additive : 씬에 추가로 로드
        SceneManager.LoadScene($"Seemless_{targetX}_{targetY}", LoadSceneMode.Additive);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 씬을 제거함
        SceneManager.UnloadSceneAsync($"Seemless_{targetX}_{targetY}");
    }
}
