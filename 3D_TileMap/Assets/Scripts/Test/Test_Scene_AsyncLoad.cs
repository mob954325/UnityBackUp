using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Scene_AsyncLoad : TestBase
{
    public string nextSceneName = "LoadSampleScene";
    AsyncOperation async;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        async = SceneManager.LoadSceneAsync(nextSceneName); // �񵿱� �ε�����
        async.allowSceneActivation = false; // �񵿱� �� �ε��� �Ϸ�Ǿ �ڵ����� �� ��ȯ�� ���� �ʴ´�.
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = true; // �񵿱� �� �ε��� �Ϸ�Ǹ� �ڵ����� �� ��ȯ�� �Ѵ�.
    }

    IEnumerator LoadCoroutine()
    {
        async = SceneManager.LoadSceneAsync(nextSceneName); // �񵿱� �ε�����
        async.allowSceneActivation = false;                 // �ڵ����� �� ��ȯ ����

        while(async.progress < 0.9f) // allowSceneActivatio�� false�� progress�� 0.9�� �ִ�( �ε��Ϸ� = 0.9 )
        {
            Debug.Log($"Progress : {async.progress}");
            yield return null;
        }

        Debug.Log("Loading Complete");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        StartCoroutine(LoadCoroutine());
    }
}
