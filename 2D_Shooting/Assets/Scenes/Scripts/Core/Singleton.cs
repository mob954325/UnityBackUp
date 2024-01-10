using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// 이 싱글톤이 초기화 되었는지 확인하기 위한 변수
    /// </summary>
    bool isInitialized = false;

    /// <summary>
    /// 종료처리에 들어갔는지 확인하는 변수
    /// </summary>
    private static bool isShutDown = false;

    /// <summary>
    /// 이 싱글톤의 객체(인스턴스)
    /// </summary>
    private static T instance = null;

    /// <summary>
    /// 이 싱들톤의 객체를 읽기 위한 프로퍼티.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutDown) // 종료 처리에 들어가면 
            {
                Debug.LogWarning("싱글톤은 이미 삭제중이다."); //경고 출력 후 null로 반환
                return null;
            }
            if (instance == null) // 객체가 없으면
            {
                T singleton = FindAnyObjectByType<T>(); // 다른 게임 오브젝트에 싱글톤이 있는지 확인
                if (singleton == null) // 없으면
                {
                    GameObject obj = new GameObject(); // 빈오브젝트 생성
                    obj.name = "Singleton";
                    singleton = obj.AddComponent<T>(); // 싱글톤 컴포넌트 추가
                }
                instance = singleton; // 다른 게임오브젝트에 있는 싱글톤이나 새로만든 싱글톤 저장
                DontDestroyOnLoad(instance.gameObject); // 씬이 사라질 때 게임 오브젝트가 삭제되지 않도록 수정
            }
            return instance;
        }
    }

    void OnApplicationQuit() // 종료될때 실행되는 함수
    {
        isShutDown = true;
    }

    void Awake()
    {
        if (instance == null) // 씬에 이미 배치된 싱글톤이 없다
        {
            instance = this as T; // 첫째 저장
            DontDestroyOnLoad(instance.gameObject); // 삭제되지 않도록 변경
        }
        else
        {
            // 이미 있으면
            if (instance != this) // 그게 자신이 아니면
            {
                Destroy(this.gameObject); // 나 자신을 삭제
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬로딩이 완료되고 실행되는 action()
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 씬이 로드되었을 때 호출될 함수
    /// </summary>
    /// <param name="scene">씬정보</param>
    /// <param name="mode">로딩모드</param>
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
    /// 싱글톤이 만들어질 때 단 한번만 호출되는 함수
    /// </summary>
    protected virtual void OnPreInitialize()
    {
        isInitialized = true;
    }

    /// <summary>
    /// 싱글톤이 만들어지고 씬이 Single로 로드(변경)될때마다 호출(additive는 안됨)
    /// </summary>
    protected virtual void OnInitialize()
    {

    }
}


// 싱글톤은 무조건 객체가 한개여야한다.
public class TestSingTon
{
    private static TestSingTon instance = null;

    public static TestSingTon Instance
    {
        get
        {
            if (instance == null) // 이전에 인스턴스가 만들어진 적이 없으면 
            {
                instance = new TestSingTon(); // 생성
            }
            return instance;
        }

    }
    private TestSingTon()
    {
        // 객체가 중복으로 생성되는 것을 방지하기 위해 생성자를 private로 한다.
    }
}