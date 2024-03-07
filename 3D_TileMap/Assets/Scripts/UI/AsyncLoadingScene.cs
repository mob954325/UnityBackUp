using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadingScene : MonoBehaviour
{
    /// <summary>
    /// ������ �ε����� ������ ���� �ҷ��� ���� �̸�
    /// </summary>
    public string nextSceneName = "LoadSampleScene";

    /// <summary>
    /// ����Ƽ���� �񵿱� ��� ó���� ���� �ʿ��� Ŭ����
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// slider�� value�� ������ �� ��
    /// </summary>
    float loadRatio;

    /// <summary>
    /// slider�� value�� �����ϴ� �ӵ�
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    /// <summary>
    /// ���� ����� �ڷ�ƾ
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// �ε� �Ϸ� ǥ�� (true�� �Ϸ�, false �̿�)
    /// </summary>
    bool loadingDone = false;
    
    // UI
    Slider loadingSlider;
    TextMeshProUGUI loadingText;

    // �Է� ó����
    PlayerInputAction inputActions; // ���ͷ����� ���� inputAction

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += Press;
        inputActions.UI.AnyKey.performed += Press;
    }

    private void OnDisable()
    {
        inputActions.UI.AnyKey.performed -= Press;
        inputActions.UI.Click.performed -= Press;
        inputActions.UI.Disable();
    }

    private void Start()
    {
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();

        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(AsyncLoadScene());
    }

    private void Update()
    {
        // �����̴��� value�� loadRatio�� �� ������ ��� �����Ѵ�.
        if(loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    /// <summary>
    /// ���콺�� Ű �������� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="context"></param>
    private void Press(InputAction.CallbackContext context)
    {
        async.allowSceneActivation = loadingDone; // loadingDone�� true�� allowSceneActivataion�� true�� �ٲ۴�.
    }

    /// <summary>
    /// ���� ����� ��� ����Ǵ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {
        // 1.
        // 0.2f�� �������� .�� ������.
        // .�� �ִ� 5�������� ������.
        // "Loading" ~ "Loading . . . . ."

        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string[] texts =
        {
            "Loading",
            "Loading .",
            "Loading . .",
            "Loading . . .",
            "Loading . . . .",
            "Loading . . . . ."
        };

        int index = 0;
        while(true)
        {
            loadingText.text = texts[index++];
            index %= texts.Length;
            yield return null; // �ڷ�ƾ ���

            yield return wait;
        }
    }

    //�񵿱�� ���� �ε��ϴ� �ڷ�ƾ
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;


        async = SceneManager.LoadSceneAsync(nextSceneName); // �񵿱� �ε� ����
        async.allowSceneActivation = false;                 // �ڵ����� ����ȯ ��Ȱ��ȭ

        while(loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;              // �ε� �������� ���� loadRatio ����
            yield return null;
        }

        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        // 2.
        // �����ִ� �����̴��� �� �� ������ ��޸���
        StopCoroutine(loadingTextCoroutine);                // �ε� �ؽ�Ʈ ���� 
        loadingText.text = "Loading\nComplete !!";          // �Ϸ� ���� ���
        loadingDone = true;                                 // �ε� �Ϸ�Ǿ��ٰ� ǥ��
    }
}