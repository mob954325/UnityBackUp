using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
{
    /// <summary>
    /// Ǯ���� ������ ������Ʈ�� ������ 
    /// </summary>
    public GameObject originalPrefab;

    /// <summary>
    /// Ǯ�� ũ��, ó�� �����ϴ� ������Ʈ�� ����, ��� ������ 2^n�� ����
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// TŸ������ ������ ������Ʈ�� �迭, ������ ��� ������Ʈ�� �ִ� �迭
    /// </summary>
    T[] pool;

    /// <summary>
    /// ���� ��밡����(��Ȱ��ȭ) ������Ʈ���� �����ϴ� ť
    /// </summary>
    Queue<T> readyQueue;

    public void Initialize()
    {
        pool = new T[poolSize]; // pool ������ ��ŭ TŸ�� �ʱ�ȭ (�����Ҵ�)
        readyQueue = new Queue<T>(poolSize); // readyQueue�� ����� capacity�� poolsize�� ����

        GenerateObject(0, poolSize, pool);

        
    }

    /// <summary>
    /// Ǯ���� ����� ������Ʈ�� �����ϴ� ����
    /// </summary>
    /// <param name="start">���� ������ ������ �ε���</param>
    /// <param name="end">���� ������ ������ �ε���</param>
    /// <param name="result">������ ������Ʈ�� �� �迭</param>
    /// <returns></returns>
    void GenerateObject(int start, int end, T[] result)
    {
        for(int i = start; i < end; i++) // ����
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab}_{i}";

            T temp = obj.GetComponent<T>();

            result[i] = temp;
            obj.SetActive(false); // ��Ȱ��ȭ
        }
    }

}
