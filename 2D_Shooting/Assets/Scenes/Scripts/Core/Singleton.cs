using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// �� �̱����� �ʱ�ȭ �Ǿ����� Ȯ���ϱ� ���� ����
    /// </summary>
    bool isInitialized = false;

    /// <summary>
    /// ����ó���� ������ Ȯ���ϴ� ����
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// �� �̱����� ��ü(�ν��Ͻ�)
    /// </summary>
    private static T instance = null;

    /// <summary>
    /// �� �̵����� ��ü�� �б� ���� ������Ƽ.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutDown) // ���� ó���� ���� 
            {
                Debug.LogWarning("�̱����� �̹� �������̴�."); //��� ��� �� null�� ��ȯ
                return null;
            }
            if (instance == null) // ��ü�� ������
            {
                T singleton = FindAnyObjectByType<T>(); // �ٸ� ���� ������Ʈ�� �̱����� �ִ��� Ȯ��
                if (singleton == null) // ������
                {
                    GameObject obj = new GameObject(); // �������Ʈ ����
                    obj.name = "Singleton";
                    singleton = obj.AddComponent<T>(); // �̱��� ������Ʈ �߰�
                }
                instance = singleton; // �ٸ� ���ӿ�����Ʈ�� �ִ� �̱����̳� ���θ��� �̱��� ����
                DontDestroyOnLoad(instance.gameObject); // ���� ����� �� ���� ������Ʈ�� �������� �ʵ��� ����
            }
            return instance;
        }
    }

    void OnApplicationQuit() // ����ɶ� ����Ǵ� �Լ�
    {
        isShutDown = true;
    }

    void Awake()
    {
        if (instance == null) // ���� �̹� ��ġ�� �̱����� ����
        {
            instance = this as T; // ù° ����
            DontDestroyOnLoad(instance.gameObject); // �������� �ʵ��� ����
        }
        else
        {
            // �̹� ������
            if (instance != this) // �װ� �ڽ��� �ƴϸ�
            {
                Destroy(this.gameObject); // �� �ڽ��� ����
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // ���ε��� �Ϸ�ǰ� ����Ǵ� action()
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// ���� �ε�Ǿ��� �� ȣ��� �Լ�
    /// </summary>
    /// <param name="scene">������</param>
    /// <param name="mode">�ε����</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isInitialized)
        {
            OnPreInitialize();
        }
        if (mode != LoadSceneMode.Additive)
        {
            OnInitialize();
        }
    }

    /// <summary>
    /// �̱����� ������� �� �� �ѹ��� ȣ��Ǵ� �Լ�
    /// </summary>
    protected virtual void OnPreInitialize()
    {
        isInitialized = true;
    }

    /// <summary>
    /// �̱����� ��������� ���� Single�� �ε�(����)�ɶ����� ȣ��(additive�� �ȵ�)
    /// </summary>
    protected virtual void OnInitialize()
    {

    }
}


// �̱����� ������ ��ü�� �Ѱ������Ѵ�.
public class TestSingTon
{
    private static TestSingTon instance = null;

    public static TestSingTon Instance
    {
        get
        {
            if (instance == null) // ������ �ν��Ͻ��� ������� ���� ������ 
            {
                instance = new TestSingTon(); // ����
            }
            return instance;
        }

    }
    private TestSingTon()
    {
        // ��ü�� �ߺ����� �����Ǵ� ���� �����ϱ� ���� �����ڸ� private�� �Ѵ�.
    }
}