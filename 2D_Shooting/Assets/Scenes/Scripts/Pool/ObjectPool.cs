using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
{
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

    public void Initialize()
    {
        pool = new T[poolSize]; // pool 사이즈 만큼 T타입 초기화 (동적할당)
        readyQueue = new Queue<T>(poolSize); // readyQueue를 만들고 capacity를 poolsize로 지정

        GenerateObject(0, poolSize, pool);

        
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
        for(int i = start; i < end; i++) // 생성
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab}_{i}";

            T temp = obj.GetComponent<T>();

            result[i] = temp;
            obj.SetActive(false); // 비활성화
        }
    }

}
