using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{ // where T : RecycleObject ->  RecycleObject상속받은 클래스만 가능하다.
    /// <summary>
    /// 풀에서 관리할 오브젝트의 프리펩 
    /// </summary>
    public GameObject originalPrefab;

    /// <summary>
    /// 풀의 크기, 처음 생성하는 오브젝트의 개수, 모든 개수는 2^n이 좋음
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// T타입으로 지정된 오브젝트의 배열, 생성된 모든 오브젝트가 있는 배열
    /// </summary>
    T[] pool;

    /// <summary>
    /// 현재 사용가능한(비활성화) 오브젝트들을 관리하는 큐
    /// </summary>
    Queue<T> readyQueue;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if(pool == null) // 풀이 아직 안만들어짐
        {
        pool = new T[poolSize]; // pool 사이즈 만큼 T타입 초기화 (동적할당)
        readyQueue = new Queue<T>(poolSize); // readyQueue를 만들고 capacity를 poolsize로 지정

            GenerateObject(0, poolSize, pool);
        }
        else
        {
            // 풀이 만들어짐 (ex> addtional loading scene or scene restart)
            foreach(T obj in pool)
            {
                obj.gameObject.SetActive(false); // 
            }
        }
    }

    /// <summary>
    /// 풀에서 사용하지 않는 오브젝트를 하나 꺼낸후 리턴하는 함수
    /// </summary>
    /// <returns>풀에서 꺼낸 오브젝트(활성화됨)</returns>
    public T GetObject()
    {
        if(readyQueue.Count > 0) // check readyqueue 
        {
            T comp = readyQueue.Dequeue(); // 남아있으면 하나 꺼냄
            comp.gameObject.SetActive(true); // 활성화
            return comp; // return
        }
        else
        {
            // readyqueue is empty == no object
            ExpandPool(); // expandPool twice
            return GetObject(); // turn self
        }
    }

    /// <summary>
    /// 풀을 두배로 늘리는 함수
    /// </summary>
    void ExpandPool()
    {
        // 최대한 일어나면 안되는 함수 -> 경고표시
        Debug.LogWarning($"{gameObject.name} 풀 사이즈 증가. {poolSize} -> {poolSize * 2}");
        int newSize = poolSize * 2;
        T[] newPool = new T[newSize];

        for(int i = 0; i< poolSize; i++) // 기존 풀에 있던거 가져오기
        {
            newPool[i] = pool[i];
        }

        GenerateObject(poolSize, newSize, newPool);
        pool = newPool; // 풀 재설정
        poolSize = newSize; // 사이즈 재설정 
    }

    /// <summary>
    /// 풀에서 사용할 오브젝트를 생성하는 변수
    /// </summary>
    /// <param name="start">새로 생성을 시작할 인덱스</param>
    /// <param name="end">새로 생성이 끝나는 인덱스</param>
    /// <param name="result">생성된 오브젝트가 들어갈 배열</param>
    /// <returns></returns>
    void GenerateObject(int start, int end, T[] result)
    {
        for(int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(originalPrefab, transform); // instantiate
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();
            comp.onDisable += () => readyQueue.Enqueue(comp); // 재활용 오브젝트가 비활성화 되면 레디큐로 되돌리기
            //readyQueue.Enqueue(comp); // add to readyqueue

            result[i] = comp; // add to array
            obj.SetActive(false); // 비활성화
        }
    }
}
